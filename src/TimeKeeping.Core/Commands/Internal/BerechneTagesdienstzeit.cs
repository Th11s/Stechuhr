using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;
using Th11s.TimeKeeping.Configuration;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Commands.Internal
{
    public record BerechneTagesdienstzeit(
        Guid ArbeitsplatzId,
        DateOnly Datum
        ) : ICommand
    { }

    internal class BerechneTagesdienstzeitHandler : CommandHandler<BerechneTagesdienstzeit, BerechneTagesdienstzeitHandler.CommandContext> {
        private readonly ApplicationDbContext _dbContext;
        private readonly TimeProvider _timeProvider;
        private readonly IOptions<StechzeitOptions> _options;


        public override ILogger Logger { get; }


        public BerechneTagesdienstzeitHandler(
            ApplicationDbContext dbContext,
            TimeProvider timeProvider,
            IOptions<StechzeitOptions> options,
            ILogger<BerechneTagesdienstzeitHandler> logger)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
            _options = options;
            Logger = logger;

            // Dieser Handler benötigt keine Authorisierung, da die Berechnung idempotent ist.
            SkipAuthorization = true;
        }


        protected override async Task<HandlerResult> ExecuteInternalAsync(BerechneTagesdienstzeit command, CommandContext context, ClaimsPrincipal? principal, CancellationToken ct)
        {
            Tagesdienstzeit tagesdienstzeit;
            if (context.Tagesdienstzeit == null)
            {
                tagesdienstzeit = new(
                    command.ArbeitsplatzId,
                    command.Datum
                )
                {
                    Sollarbeitszeit = context.Arbeitsplatz.Standarddienstzeit
                };

                _dbContext.Tagesdienstzeiten.Add(tagesdienstzeit);
                await _dbContext.SaveChangesAsync(ct);
            }
            else {
                tagesdienstzeit = context.Tagesdienstzeit;
            }

            var probleme = new List<string>();
            var zeitpaare = PaareUndValidiereZeitstempel(context.Stechzeiten, probleme);

            var zeiten = BerechneArbeitsUndPausenzeit(zeitpaare);


            var minimumPausenzeit = _options.Value.GetMinimalePause(zeiten.Arbeitszeit);
            if (minimumPausenzeit > zeiten.Pausenzeit)
            {
                tagesdienstzeit.Pausenzeit = minimumPausenzeit;
                tagesdienstzeit.HatPausezeitminimum = true;
            }
            else
            {
                tagesdienstzeit.Pausenzeit = zeiten.Pausenzeit;
                tagesdienstzeit.HatPausezeitminimum = false;
            }


            tagesdienstzeit.Arbeitszeit = zeiten.Arbeitszeit - tagesdienstzeit.Pausenzeit;
            tagesdienstzeit.Probleme = [.. probleme];

            

            //TODO: "Standardgutschriften" anwenden
            // tagesdienstzeit.Arbeitszeitgutschrift = context.Abteilung.Arbeitszeitgutschriften

            tagesdienstzeit.LastModified = context.Timestamp;


            await _dbContext.SaveChangesAsync(ct);
            return HandlerResult.Success();
        }

        private (TimeSpan Arbeitszeit, TimeSpan Pausenzeit) BerechneArbeitsUndPausenzeit(List<(Zeiterfassung Anfang, Zeiterfassung Ende)> zeitpaare)
        {
            var arbeitszeit = TimeSpan.Zero;
            var pausenzeit = TimeSpan.Zero;

            foreach(var (anfang, ende) in zeitpaare)
            {
                var dauer = ende.Zeitstempel - anfang.Zeitstempel;

                if (anfang.IstArbeitsbeginn)
                    arbeitszeit += dauer;

                if (anfang.IstPausenbeginn)
                    pausenzeit += dauer;
            }

            return (arbeitszeit, pausenzeit);
        }

        private List<(Zeiterfassung Anfang, Zeiterfassung Ende)> PaareUndValidiereZeitstempel(IEnumerable<Zeiterfassung> stechzeiten, List<string> probleme)
        {
            var zeitpaare = new List<(Zeiterfassung Anfang, Zeiterfassung Ende)>();
            var stack = new Stack<Zeiterfassung>();
            foreach(var st in stechzeiten)
            {
                if (stack.Count == 0)
                {
                    if (st.IstArbeitsbeginn)
                    {
                        stack.Push(st);
                    }
                    else
                    {
                        probleme.Add("Zeitberechnung:Arbeitsbeginn");
                    }
                }

                else
                {
                    if (st.IstPausenbeginn && stack.Peek().IstArbeitsbeginn)
                    {
                        stack.Push(st);
                    }
                    else {
                        probleme.Add("Zeitberechnung:Pausenanfang");
                    }

                    if (st.IstPausenende && stack.Peek().IstPausenbeginn)
                    {
                        zeitpaare.Add((stack.Pop(), st));
                    }
                    else
                    {
                        probleme.Add("Zeitberechnung:Pausenende");
                    }

                    if(st.IstArbeitsende && stack.Peek().IstArbeitsbeginn)
                    {
                        zeitpaare.Add((stack.Pop(), st));
                    }
                    else
                    {
                        probleme.Add("Zeitberechnung:Arbeitsende");
                    }
                }
            }

            return zeitpaare;
        }

        private TimeSpan BerechneDauer(Queue<DateTimeOffset> anfange, Queue<DateTimeOffset> enden, List<string> probleme)
        {
            TimeSpan result = TimeSpan.Zero;
            while(anfange.Count > 0 && enden.Count > 0)
            {
                var (anfang, ende) = (anfange.Dequeue(), enden.Dequeue());
                if (ende < anfang)
                    probleme.Add("Zeitberechnung:EndeVorAnfang");

                result += ende - anfang;
            }

            if (anfange.Count > 0)
                probleme.Add("Zeitberechnung:AnfangszeitOhneEnde");

            if (enden.Count > 0)
                probleme.Add("Zeitberechung:EndeOhneAnfangszeit");

            return result;
        }

        protected override async Task<CommandContext> InitializeAsync(BerechneTagesdienstzeit command, ClaimsPrincipal? principal, CancellationToken ct)
        {
            var timestamp = _timeProvider.GetUtcNow();

            var arbeitsplatz = await _dbContext.Arbeitsplaetze
                .Include(x => x.Abteilung)
                .Where(x => x.Id == command.ArbeitsplatzId)
                .FirstOrDefaultAsync(ct);

            if (arbeitsplatz == null)
                throw new CommandException(HandlerResult.NotValid("Validation:Arbeitsplatz:NotFound"));

            var tagesdienstzeit = await _dbContext.Tagesdienstzeiten
                .Where(x =>
                    x.ArbeitsplatzId == command.ArbeitsplatzId &&
                    x.Datum == command.Datum)
                .FirstOrDefaultAsync(ct);

            var stechzeiten = await _dbContext.Zeiterfassung
                .Where(x =>
                    x.ArbeitsplatzId == command.ArbeitsplatzId &&
                    x.Datum == command.Datum &&
                    !x.IstEntfernt)
                .OrderBy(x => x.Zeitstempel)
                .ToListAsync(ct);

            return new(timestamp, arbeitsplatz, tagesdienstzeit, stechzeiten);
        }


        internal record CommandContext(
            DateTimeOffset Timestamp,
            Arbeitsplatz Arbeitsplatz,
            Tagesdienstzeit? Tagesdienstzeit,
            List<Zeiterfassung> Stechzeiten);
    }
}

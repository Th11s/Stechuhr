using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        string ArbeitnehmerId,
        int AbteilungsId,
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
                    command.ArbeitnehmerId,
                    command.AbteilungsId,
                    command.Datum,
                    context.Arbeitnehmer.Standarddienstzeit
                );

                _dbContext.Tagesdienstzeiten.Add(tagesdienstzeit);
                await _dbContext.SaveChangesAsync(ct);
            }
            else {
                tagesdienstzeit = context.Tagesdienstzeit;
            }

            var zeitLookup = context.Stechzeiten.Select(x => x.Stechzeit).ToLookup(x => x.Typ);
            var arbeitsAnfange = new Queue<Stechzeit>(zeitLookup[StechTyp.Arbeitsbeginn].OrderBy(x => x.Zeitstempel));
            var arbeitsEnden = new Queue<Stechzeit>(zeitLookup[StechTyp.Arbeitsende].OrderBy(x => x.Zeitstempel));
            var pausenAnfange = new Queue<Stechzeit>(zeitLookup[StechTyp.Pausenbeginn].OrderBy(x => x.Zeitstempel));
            var pausenEnden = new Queue<Stechzeit>(zeitLookup[StechTyp.Pausenende].OrderBy(x => x.Zeitstempel));

            var probleme = new List<string>();

            var pausenzeit = BerechneDauer(arbeitsAnfange, arbeitsEnden, probleme);
            var arbeitszeit = BerechneDauer(arbeitsAnfange, arbeitsEnden, probleme);
            
            tagesdienstzeit.Arbeitszeit = arbeitszeit - pausenzeit;
            tagesdienstzeit.Probleme = probleme.ToArray();

            var minimumPausenzeit = _options.Value.GetMinimalePause(arbeitszeit);
            if(minimumPausenzeit > pausenzeit)
            {
                tagesdienstzeit.Pausenzeit = minimumPausenzeit;
                tagesdienstzeit.HatPausezeitminimum = true;
            }
            else
            {
                tagesdienstzeit.Pausenzeit = pausenzeit;
                tagesdienstzeit.HatPausezeitminimum = false;
            }

            //TODO: "Standardgutschriften" anwenden
            // tagesdienstzeit.Arbeitszeitgutschrift = context.Abteilung.Arbeitszeitgutschriften

            tagesdienstzeit.LastModified = context.Timestamp;


            await _dbContext.SaveChangesAsync(ct);
            return HandlerResult.Success();
        }

        private TimeSpan BerechneDauer(Queue<Stechzeit> anfange, Queue<Stechzeit> enden, List<string> probleme)
        {
            TimeSpan result = TimeSpan.Zero;
            while(anfange.Count > 0 && enden.Count > 0)
            {
                var (anfang, ende) = (anfange.Dequeue(), enden.Dequeue());
                if (ende.Zeitstempel < anfang.Zeitstempel)
                    probleme.Add("Zeitberechnung:EndeVorAnfang");

                result += ende.Zeitstempel - anfang.Zeitstempel;
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

            var arbeitnehmer = await _dbContext.Arbeitnehmer
                .Where(x => x.Id == command.ArbeitnehmerId)
                .FirstOrDefaultAsync(ct);

            if (arbeitnehmer == null)
                throw new CommandException(HandlerResult.NotValid("Validation:Arbeitnehmer:NotFound"));

            var abteilung = await _dbContext.Abteilung
                .Where(x => x.Id == command.AbteilungsId)
                .FirstOrDefaultAsync(ct);

            if (abteilung == null)
                throw new CommandException(HandlerResult.NotValid("Validation:Abteilung:NotFound"));

            var tagesdienstzeit = await _dbContext.Tagesdienstzeiten
                .Where(x =>
                    x.ArbeitnehmerId == command.ArbeitnehmerId &&
                    x.AbteilungsId == command.AbteilungsId &&
                    x.Datum == command.Datum)
                .FirstOrDefaultAsync(ct);

            var stechzeiten = await _dbContext.Zeiterfassung
                .Where(x =>
                    x.ArbeitnehmerId == command.ArbeitnehmerId &&
                    x.AbteilungsId == command.AbteilungsId &&
                    x.Stechzeit.Datum == command.Datum &&
                    !x.IstEntfernt)
                .ToListAsync(ct);

            return new(timestamp, arbeitnehmer, abteilung, tagesdienstzeit, stechzeiten);
        }


        internal record CommandContext(
            DateTimeOffset Timestamp,
            Arbeitnehmer Arbeitnehmer,
            Abteilung Abteilung,
            Tagesdienstzeit? Tagesdienstzeit,
            List<Zeiterfassung> Stechzeiten);
    }
}

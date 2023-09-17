using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<StechzeitOptions> _options;

        public override ILogger Logger { get; }


        public BerechneTagesdienstzeitHandler(
            ApplicationDbContext dbContext,
            IOptions<StechzeitOptions> options,
            ILogger<BerechneTagesdienstzeitHandler> logger)
        {
            _dbContext = dbContext;
            _options = options;
            Logger = logger;
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
            }
            else {
                tagesdienstzeit = context.Tagesdienstzeit;
            }

            var zeitLookup = context.Stechzeiten.Select(x => x.Stechzeit).ToLookup(x => x.Typ);
            var arbeitsAnfange = new Queue<Stechzeit>(zeitLookup[StechTyp.Arbeitsbeginn].OrderBy(x => x.Uhrzeit));
            var arbeitsEnden = new Queue<Stechzeit>(zeitLookup[StechTyp.Arbeitsende].OrderBy(x => x.Uhrzeit));
            var pausenAnfange = new Queue<Stechzeit>(zeitLookup[StechTyp.Pausenbeginn].OrderBy(x => x.Uhrzeit));
            var pausenEnden = new Queue<Stechzeit>(zeitLookup[StechTyp.Pausenende].OrderBy(x => x.Uhrzeit));

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

            
            await _dbContext.SaveChangesAsync(ct);
            return HandlerResult.Success();
        }

        private TimeSpan BerechneDauer(Queue<Stechzeit> anfange, Queue<Stechzeit> enden, List<string> probleme)
        {
            TimeSpan result = TimeSpan.Zero;
            while(anfange.Count > 0 && enden.Count > 0)
            {
                var (anfang, ende) = (anfange.Dequeue(), enden.Dequeue());
                if (ende.Uhrzeit < anfang.Uhrzeit)
                    probleme.Add("Zeitberechnung:EndeVorAnfang");

                result += ende.Uhrzeit - anfang.Uhrzeit;
            }

            if (anfange.Count > 0)
                probleme.Add("Zeitberechnung:AnfangszeitOhneEnde");

            if (enden.Count > 0)
                probleme.Add("Zeitberechung:EndeOhneAnfangszeit");

            return result;
        }

        protected override async Task<CommandContext> InitializeAsync(BerechneTagesdienstzeit command, ClaimsPrincipal? principal, CancellationToken ct)
        {
            var arbeitnehmer = await _dbContext.Arbeitnehmer
                .Where(x => x.Id == command.ArbeitnehmerId)
                .FirstOrDefaultAsync(ct);

            if (arbeitnehmer == null)
                throw new CommandException(HandlerResult.NotValid("Validation:Arbeitnehmer:NotFound"));

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

            return new(arbeitnehmer, tagesdienstzeit, stechzeiten);
        }


        internal record CommandContext(
            Arbeitnehmer Arbeitnehmer,
            Tagesdienstzeit? Tagesdienstzeit,
            List<Zeiterfassung> Stechzeiten);
    }
}

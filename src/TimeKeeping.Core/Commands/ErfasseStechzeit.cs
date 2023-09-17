using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Th11s.TimeKeeping.Configuration;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Commands
{
    public record ErfasseStechzeit(
        DateOnly Datum,
        TimeOnly Uhrzeit,
        StechTyp Typ,

        int ArbeitnehmerId,
        int AbteilungsId
        ) : ICommand
    { }

    internal class ErfasseStechzeitHandler : CommandHandler<ErfasseStechzeit>
    {
        private readonly TimeProvider _timeProvider;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOptions<StechzeitOptions> _options;

        public override ILogger Logger { get; }

        public ErfasseStechzeitHandler(
            TimeProvider timeProvider,
            ApplicationDbContext dbContext,
            ILogger<ErfasseStechzeitHandler> logger,
            IOptions<StechzeitOptions> options)
        {
            _timeProvider = timeProvider;
            _dbContext = dbContext;
            Logger = logger;
            _options = options;
        }

        protected override async Task<HandlerResult> ExecuteInternalAsync(ErfasseStechzeit command, ClaimsPrincipal? principal, CancellationToken ct)
        {
            var stechzeit = command.Datum.ToDateTime(command.Uhrzeit);
            var istNachgebucht = Math.Abs((stechzeit - _timeProvider.GetLocalNow()).TotalMinutes) > _options.;

            var entry = new Zeiterfassung
            {
                ArbeitnehmerId = command.ArbeitnehmerId,
                AbteilungsId = command.AbteilungsId,

                Stechzeit = new Stechzeit
                {
                    Datum = command.Datum,
                    Uhrzeit = command.Uhrzeit,
                    Typ = command.Typ,
                },

                HatNachbuchungen = istNachgebucht,
                // TODO: Nachverfolgung = 
            };

            _dbContext.Zeiterfassung.Add(entry);
            await _dbContext.SaveChangesAsync(ct);


            _dbContext.UpdateTagesdienstzeit()
            // TODO: prüfen, ob das ein korrektes Update-Statement für einen existierenden Eintrag erzeugt.
            var tagesdienszeit = new Tagesdienstzeit
            {
                ArbeitnehmerId = command.ArbeitnehmerId,
                AbteilungsId = command.AbteilungsId,
                Datum = command.Datum,

                HatAusstehendeBerechnung = true,
            };

            _dbContext.Update(tagesdienszeit);

            await _dbContext.SaveChangesAsync(ct);
            return HandlerResult.Success();
        }
    }
}

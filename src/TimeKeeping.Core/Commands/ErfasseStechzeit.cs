using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Commands
{
    public record ErfasseStechzeit(
        DateOnly Datum,
        TimeOnly Uhrzeit,
        StechTyp Typ,

        int AbteilungsId
        ) : ICommand
    { }

    public class ErfasseStechzeitHandler : CommandHandler<ErfasseStechzeit>
    {
        private readonly TimeProvider _timeProvider;
        private readonly ApplicationDbContext _dbContext;

        public override ILogger Logger { get; }

        public ErfasseStechzeitHandler(
            TimeProvider timeProvider,
            ApplicationDbContext dbContext,
            ILogger<ErfasseStechzeitHandler> logger)
        {
            _timeProvider = timeProvider;
            _dbContext = dbContext;
            Logger = logger;
        }

        protected override async Task<HandlerResult> ExecuteInternalAsync(ErfasseStechzeit command, ClaimsPrincipal? principal, CancellationToken ct)
        {
            var arbeitnehmerId = 1; // TODO: command.GetArbeitnehmerId();
            var stechzeit = command.Datum.ToDateTime(command.Uhrzeit);
            var istNachgebucht = Math.Abs((stechzeit - _timeProvider.GetLocalNow()).TotalMinutes) > 5;

            var entry = new Zeiterfassung
            {
                ArbeitnehmerId = arbeitnehmerId,
                AbteilungsId = command.AbteilungsId,

                Stechzeit = new Stechzeit
                {
                    Datum = command.Datum,
                    Uhrzeit = command.Uhrzeit,
                    Typ = command.Typ,
                },

                IstNachgebucht = istNachgebucht,
                // TODO: Nachverfolgung = 
            };

            _dbContext.Zeiterfassung.Add(entry);

            // TODO: prüfen, ob das ein korrektes Update-Statement für einen existierenden Eintrag erzeugt.
            var tagesdienszeit = new Tagesdienstzeit
            {
                ArbeitnehmerId = arbeitnehmerId,
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

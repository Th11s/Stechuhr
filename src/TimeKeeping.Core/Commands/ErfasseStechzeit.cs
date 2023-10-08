using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Th11s.TimeKeeping.Commands.Internal;
using Th11s.TimeKeeping.Configuration;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;
using Th11s.TimeKeeping.SharedModel.Primitives;
using Th11s.TimeKeeping.SharedModel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Th11s.TimeKeeping.Commands
{
    public record ErfasseStechzeit(
        Guid? Id,
        Guid ArbeitsplatzId,

        DateOnly Datum,
        DateTimeOffset Stechzeit,
        Stempeltyp Typ
        ) : ICommand
    { }

    internal class ErfasseStechzeitHandler : CommandHandler<ErfasseStechzeit, Zeiterfassung?>
    {
        private readonly TimeProvider _timeProvider;
        private readonly ApplicationDbContext _dbContext;
        private readonly ICommandHandler<BerechneTagesdienstzeit> _tagesdienstzeitBerechnung;
        private readonly IOptions<StechzeitOptions> _options;

        public override ILogger Logger { get; }

        public ErfasseStechzeitHandler(
            TimeProvider timeProvider,
            ApplicationDbContext dbContext,
            ICommandHandler<BerechneTagesdienstzeit> tagesdienstzeitBerechnung,
            ILogger<ErfasseStechzeitHandler> logger,
            IOptions<StechzeitOptions> options)
        {
            _timeProvider = timeProvider;
            _dbContext = dbContext;
            _tagesdienstzeitBerechnung = tagesdienstzeitBerechnung;
            Logger = logger;
            _options = options;
        }


        protected override async Task<Zeiterfassung?> InitializeAsync(ErfasseStechzeit command, ClaimsPrincipal? principal, CancellationToken ct)
        {
            var context = await _dbContext.Zeiterfassung
                .Where(x => x.Id == command.Id && x.ArbeitsplatzId == command.ArbeitsplatzId)
                .FirstOrDefaultAsync(ct);

            return context;
        }


        protected override Task<bool> AuthorizeAsync(ErfasseStechzeit command, Zeiterfassung? context, ClaimsPrincipal? principal, CancellationToken ct)
        {
            if (principal?.GetArbeitsplatzIds().Contains(command.ArbeitsplatzId.ToString()) == true)
                return Task.FromResult(true);

#if DEBUG
            Logger.LogError("AUTH NOT IMPLEMENTED!");
            return Task.FromResult(true);
#endif

            return Task.FromResult(false);
        }

        protected override async Task<HandlerResult> ExecuteInternalAsync(ErfasseStechzeit command, Zeiterfassung? context, ClaimsPrincipal? principal, CancellationToken ct)
        {
            // TODO: Mit Aaron besprechen, was gespeichert werden sollte. Vermutlich "lokales Datum" + DateTimeOffset als "echter" zeitstempel.
            var stechzeit = command.Stechzeit;
            var uhrabweichung = (stechzeit.ToUniversalTime() - _timeProvider.GetUtcNow()).TotalMinutes;
            var maxUhrabweichung = _options.Value.Nachbuchungsschwelle.TotalMinutes;

            var istNachgebucht = uhrabweichung > maxUhrabweichung;
            var istVorausgebucht = uhrabweichung < maxUhrabweichung;

            var entry = context ?? new Zeiterfassung(
                command.ArbeitsplatzId,
                command.Datum,
                command.Stechzeit,
                command.Typ)
            {
                IstNachbuchung = istNachgebucht,
                IstVorausbuchung = istVorausgebucht,

                LastModified = _timeProvider.GetUtcNow()
            };

            //TODO: Nachverfolgung aktualisieren

            _dbContext.Zeiterfassung.Add(entry);
            await _dbContext.SaveChangesAsync(ct);

            try
            {
                await _tagesdienstzeitBerechnung.ExecuteAsync(new BerechneTagesdienstzeit(entry.ArbeitsplatzId, entry.Datum), ct);
            }
            catch
            {
                //TODO: Tagesdienstzeit per "maintenance" erneut berechnen.
                // Benutzer informieren, dass seine Dienstzeit evtl. unpräzise ist.
            }
            
            return HandlerResult.Success();
        }
    }
}

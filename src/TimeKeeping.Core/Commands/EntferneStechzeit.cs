using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Th11s.TimeKeeping.Commands.Internal;
using Th11s.TimeKeeping.Configuration;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;
using Th11s.TimeKeeping.SharedModel.Extensions;

namespace Th11s.TimeKeeping.Commands
{
    public record EntferneStechzeit(Guid Id, Guid ArbeitsplatzId) : ICommand;

    internal class EntferneStechzeitHandler : CommandHandler<EntferneStechzeit, Zeiterfassung>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly TimeProvider _timeProvider;
        private readonly ICommandHandler<BerechneTagesdienstzeit> _tagesdienstzeitBerechnung;

        public override ILogger Logger { get; }

        private readonly IOptions<StechzeitOptions> _options;

        public EntferneStechzeitHandler(
            TimeProvider timeProvider,
            ApplicationDbContext dbContext,
            ICommandHandler<BerechneTagesdienstzeit> tagesdienstzeitBerechnung,
            ILogger<EntferneStechzeitHandler> logger,
            IOptions<StechzeitOptions> options)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
            _tagesdienstzeitBerechnung = tagesdienstzeitBerechnung;
            Logger = logger;
            _options = options;
        }


        protected override async Task<Zeiterfassung> InitializeAsync(EntferneStechzeit command, ClaimsPrincipal? principal, CancellationToken ct)
        {
            var context = await _dbContext.Zeiterfassung
                .Where(x => x.Id == command.Id && x.ArbeitsplatzId == command.ArbeitsplatzId)
                .Where(x => !x.IstEntfernt)
                .FirstOrDefaultAsync(ct);

            if (context == null)
                throw new CommandException(HandlerResult.NotFound());

            return context;
        }


        protected override Task<bool> AuthorizeAsync(EntferneStechzeit command, Zeiterfassung context, ClaimsPrincipal? principal, CancellationToken ct)
        {
            if (principal?.GetArbeitsplatzIds().Contains(command.ArbeitsplatzId.ToString()) == true)
                return Task.FromResult(true);

#if DEBUG
            Logger.LogError("AUTH NOT IMPLEMENTED!");
            return Task.FromResult(true);
#endif

            return Task.FromResult(false);
        }


        protected override async Task<HandlerResult> ExecuteInternalAsync(EntferneStechzeit command, Zeiterfassung context, ClaimsPrincipal? principal, CancellationToken ct)
        {
            context.IstEntfernt = true;

            //TODO: context.Nachverfolgung aktualisieren

            await _dbContext.SaveChangesAsync(ct);
            return HandlerResult.Success();
        }
    }
}

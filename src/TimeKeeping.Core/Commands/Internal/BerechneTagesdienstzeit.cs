using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Th11s.TimeKeeping.Data;

namespace Th11s.TimeKeeping.Commands.Internal
{
    public record BerechneTagesdienstzeit(
        int ArbeitnehmerId,
        int AbteilungsId,
        DateOnly Datum
        ) : ICommand
    { }

    internal class BerechneTagesdienstzeitHandler : CommandHandler<BerechneTagesdienstzeit, BerechneTagesdienstzeitHandler.CommandContext> {
        private readonly ApplicationDbContext _dbContext;

        internal class CommandContext
        {

        }


        public override ILogger Logger { get; }

        public BerechneTagesdienstzeitHandler(
            ApplicationDbContext dbContext,
            ILogger<BerechneTagesdienstzeitHandler> logger)
        {
            _dbContext = dbContext;
            Logger = logger;
        }


        protected override Task<HandlerResult> ExecuteInternalAsync(BerechneTagesdienstzeit command, CommandContext context, ClaimsPrincipal? principal, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


        protected override Task<CommandContext> InitializeAsync(BerechneTagesdienstzeit command, ClaimsPrincipal? principal, CancellationToken ct)
        {
            
        }
    }
}

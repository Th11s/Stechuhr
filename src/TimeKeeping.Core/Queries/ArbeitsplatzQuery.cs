using JGUZDV.CQRS.Queries;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Th11s.TimeKeeping.SharedModel.Extensions;

namespace Th11s.TimeKeeping.Queries
{
    public abstract class ArbeitsplatzQuery(Guid arbeitsplatzId)
    {
        public Guid ArbeitsplatzId { get; } = arbeitsplatzId;
    }

    public abstract class ArbeitsplatzQueryHandler<TQuery, TValue> : QueryHandler<TQuery, TValue>
        where TQuery: ArbeitsplatzQuery, IQuery<TValue>
    {
        protected override Task<bool> AuthorizeExecuteAsync(TQuery query, ClaimsPrincipal? principal, CancellationToken ct)
        {
            if (principal?.GetArbeitsplatzIds().Contains(query.ArbeitsplatzId.ToString()) == true)
                return Task.FromResult(true);

#if DEBUG
            Logger.LogError("AUTH NOT IMPLEMENTED!");
            return Task.FromResult(true);
#endif

            return Task.FromResult(false);
        }
    }
}

using JGUZDV.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.SharedModel.Web;

namespace Th11s.TimeKeeping.Queries;

public class LadeTagesdienstzeiten(Guid arbeitsplatzId, DateOnly datum) : IQuery<Tagessicht>
{
    public Guid ArbeitsplatzId { get; set; } = arbeitsplatzId;
    public DateOnly Datum { get; } = datum;

    public QueryResult<Tagessicht> Result { get; set; } = Results.Empty;
}

internal class LadeTagesdienstzeitenHandler : QueryHandler<LadeTagesdienstzeiten, Tagessicht>
{
    private readonly ApplicationDbContext _dbContext;

    public override ILogger Logger { get; }

    public LadeTagesdienstzeitenHandler(
        ApplicationDbContext dbContext,
        ILogger<LadeTagesdienstzeitenHandler> logger)
    {
        _dbContext = dbContext;
        Logger = logger;
    }

    protected override Task<bool> AuthorizeExecuteAsync(LadeTagesdienstzeiten query, ClaimsPrincipal? principal, CancellationToken ct)
    {
#if DEBUG
        Logger.LogError("AUTH NOT IMPLEMENTED!");
        return Task.FromResult(true);
#endif
    }


    protected override async Task<QueryResult<Tagessicht>> ExecuteInternalAsync(LadeTagesdienstzeiten query, ClaimsPrincipal? principal, CancellationToken ct)
    {
        var tagesdienszeiten = await _dbContext.Tagesdienstzeiten
            .Where(x =>
                x.ArbeitsplatzId == query.ArbeitsplatzId &&
                x.Datum == query.Datum
            )
            .FirstOrDefaultAsync(ct);

        var stechzeiten = await _dbContext.Zeiterfassung
            .Where(x =>
                x.ArbeitsplatzId == query.ArbeitsplatzId &&
                x.Datum == query.Datum
            )
            .Select(x => new Stechzeit(
                x.Id,
                x.Zeitstempel,
                x.Stempeltyp,
                x.IstNachbuchung,
                x.IstVorausbuchung,
                x.HatAnpassungen,
                x.IstEntfernt
                ))
            .ToArrayAsync(ct);

        return new Tagessicht(
            query.Datum,
            stechzeiten,
            tagesdienszeiten != null
                ? new Tageswerte(
                    tagesdienszeiten.Arbeitszeit,
                    tagesdienszeiten.Pausenzeit,
                    tagesdienszeiten.HatPausezeitminimum,
                    tagesdienszeiten.Sollarbeitszeit,
                    tagesdienszeiten.Arbeitszeitgutschrift,
                    tagesdienszeiten.Zeitsaldo,
                    tagesdienszeiten.Probleme,
                    tagesdienszeiten.HatProbleme)
                : null
        );
            
    }
}

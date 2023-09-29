using JGUZDV.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.SharedModel.Web;

namespace Th11s.TimeKeeping.Queries;

public class LadeTagesdienstzeiten : IQuery<Tagessicht>
{
    public LadeTagesdienstzeiten(DateOnly datum, string arbeitnehmerId, int abteilungsId)
    {
        Datum = datum;
        ArbeitnehmerId = arbeitnehmerId ?? throw new ArgumentNullException(nameof(arbeitnehmerId));
        AbteilungsId = abteilungsId;
    }

    public DateOnly Datum { get; }

    public string ArbeitnehmerId { get; }
    public int AbteilungsId { get; }

    public QueryResult<Tagessicht> Result { get; set; } = null!;
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

    protected override async Task<QueryResult<Tagessicht>> ExecuteInternalAsync(LadeTagesdienstzeiten query, ClaimsPrincipal? principal, CancellationToken ct)
    {
        var tagesdienszeiten = await _dbContext.Tagesdienstzeiten
            .Where(x =>
                x.ArbeitnehmerId == query.ArbeitnehmerId &&
                x.AbteilungsId == query.AbteilungsId &&
                x.Datum == query.Datum
            )
            .FirstOrDefaultAsync(ct);

        var stechzeiten = await _dbContext.Zeiterfassung
            .Where(x => 
                x.ArbeitnehmerId == query.ArbeitnehmerId &&
                x.AbteilungsId == query.AbteilungsId &&
                x.Datum == query.Datum
            )
            .Select(x => new Stechzeit(
                x.Uuid,
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

using JGUZDV.CQRS;
using JGUZDV.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.SharedModel.Web;

namespace Th11s.TimeKeeping.Queries
{
    public class LadeMonatsdienstzeit(Guid arbeitsplatzId, int jahr, int monat) 
        : ArbeitsplatzQuery(arbeitsplatzId), IQuery<Monatssicht>
    {
        public int Jahr { get; } = jahr;
        public int Monat { get; } = monat;

        public QueryResult<Monatssicht> Result { get; set; } = Results.Empty;
    }

    internal class LadeMonatsdienstzeitHandler : ArbeitsplatzQueryHandler<LadeMonatsdienstzeit, Monatssicht>
    {
        private readonly ApplicationDbContext _dbContext;

        public override ILogger Logger { get; }

        public LadeMonatsdienstzeitHandler(
            ApplicationDbContext dbContext,
            ILogger<LadeMonatsdienstzeitHandler> logger)
        {
            _dbContext = dbContext;
            Logger = logger;
        }

        protected override async Task<QueryResult<Monatssicht>> ExecuteInternalAsync(LadeMonatsdienstzeit query, ClaimsPrincipal? principal, CancellationToken ct)
        {
            if (query.Monat is < 1 or > 12)
                return HandlerResult.NotValid("Monat:OutOfRange");

            var dienstzeiten = await _dbContext.Tagesdienstzeiten
                .Where(x => x.ArbeitsplatzId == query.ArbeitsplatzId)
                .Where(x => x.Datum.Month == query.Monat && x.Datum.Year == query.Jahr)
                .Select(x => new
                {
                    Datum = x.Datum,
                    Tageswert = new Tageswerte(x.Arbeitszeit, x.Pausenzeit, x.HatPausezeitminimum, x.Sollarbeitszeit, x.Arbeitszeitgutschrift, x.Zeitsaldo, x.Probleme, x.HatProbleme)
                })
                .ToListAsync(ct);

            var tageswerte = new Tageswerte[31];
            foreach(var d in dienstzeiten)
                tageswerte[d.Datum.Day] = d.Tageswert;

            return new Monatssicht(query.Jahr, query.Monat, tageswerte);
        }
    }
}

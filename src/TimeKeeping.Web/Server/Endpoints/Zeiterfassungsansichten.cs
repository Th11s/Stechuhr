
using JGUZDV.CQRS.AspNetCore.Http;
using JGUZDV.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Th11s.TimeKeeping.Queries;

namespace TimeKeeping.Web.Server.Endpoints
{
    public static class Zeiterfassungsansichten
    {
        internal static async Task<IResult> Tagesansicht(
            int jahr, int monat, int tag, [FromQuery(Name = "aid")] int? abteilungsId,
            HttpContext context, CancellationToken ct,
            [FromServices] IQueryHandler<LadeTagesdienstzeiten> queryHandler)
        {
            DateOnly datum;
            try
            {
                datum = new DateOnly(jahr, monat, tag);
            }
            catch
            {
                return Results.BadRequest();
            }

            var result = await queryHandler.ExecuteQuery(new LadeTagesdienstzeiten(datum, "TODO", abteilungsId ?? 0), context.User, ct);
            return result.ToHttpResult();
        }
    }
}

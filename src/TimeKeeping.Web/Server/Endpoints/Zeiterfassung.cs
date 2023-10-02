
using JGUZDV.CQRS.AspNetCore.Http;
using JGUZDV.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Th11s.TimeKeeping.Queries;
using Th11s.TimeKeeping.SharedModel.Primitives;
using Th11s.TimeKeeping.Data.Entities;

namespace TimeKeeping.Web.Server.Endpoints
{
    public class Zeiterfassung
    {
        internal static async Task<IResult> Tagesansicht(
            Guid arbeitsplatzId,
            int jahr, int monat, int tag, 
            [FromServices] UserManager<User> userManager,
            [FromServices] IQueryHandler<LadeTagesdienstzeiten> queryHandler,
            HttpContext context, CancellationToken ct)
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

            var result = await queryHandler.ExecuteQuery(new LadeTagesdienstzeiten(arbeitsplatzId, datum), context.User, ct);
            return result.ToHttpResult();
        }

        internal static async Task<IResult> Monatsansicht(
            Guid arbeitsplatzId,
            int jahr, int monat,
            [FromServices] UserManager<User> userManager,
            HttpContext context, CancellationToken ct)
        {
            return Results.BadRequest("TODO: Not Implemented");
        }

        internal static async Task<IResult> Jahresansicht(
            Guid arbeitsplatzId,
            int jahr,
            HttpContext context, CancellationToken ct)
        {
            return Results.BadRequest("TODO: Not Implemented");
        }

        internal class ZeitstempelRequest
        {
            public Guid ArbeitsplatzId { get; init; }
            public required DateOnly Datum { get; init; }
            public required Stempeltyp Stempeltyp { get; init; }
            
            public DateTimeOffset? Zeitstempel { get; init; }
        }

        internal static async Task<IResult> ErfasseZeitstempel(
            [FromBody] ZeitstempelRequest request,
            [FromServices] UserManager<User> userManager,
            HttpContext context, CancellationToken ct)
        {
            var userId = userManager.GetUserId(context.User);

            return Results.BadRequest("TODO: Not Implemented");
        }
    }
}

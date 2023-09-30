
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
            int jahr, int monat, int tag, 
            string abteilung,
            [FromQuery] string? arbeitnehmer,
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

            var result = await queryHandler.ExecuteQuery(new LadeTagesdienstzeiten(datum, arbeitnehmer ?? userManager.GetUserId(context.User), abteilung), context.User, ct);
            return result.ToHttpResult();
        }

        internal static async Task<IResult> Monatsansicht(
            int jahr, int monat,
            string abteilung,
            [FromQuery] string? arbeitnehmer,
            [FromServices] UserManager<User> userManager,
            HttpContext context, CancellationToken ct)
        {
            return Results.BadRequest("TODO: Not Implemented");
        }

        internal static async Task<IResult> Jahresansicht(
            int jahr,
            string abteilung,
            [FromQuery] string? arbeitnehmer,
            HttpContext context, CancellationToken ct)
        {
            return Results.BadRequest("TODO: Not Implemented");
        }

        internal class ZeitstempelRequest
        {
            public required DateOnly Datum { get; init; }
            public required Stempeltyp Stempeltyp { get; init; }
            
            public DateTimeOffset? Zeitstempel { get; init; }

            public string? ArbeitnehmerId { get; init; }
            public int? AbteilungsId { get; init; }
        }

        internal static async Task<IResult> ErfasseZeitstempel(
            [FromBody] ZeitstempelRequest request,
            [FromQuery] string? arbeitnehmer,
            [FromServices] UserManager<User> userManager,
            HttpContext context, CancellationToken ct)
        {
            var userId = userManager.GetUserId(context.User);

            return Results.BadRequest("TODO: Not Implemented");
        }
    }
}

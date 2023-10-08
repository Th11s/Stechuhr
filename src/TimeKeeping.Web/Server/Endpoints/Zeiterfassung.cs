
using JGUZDV.CQRS.AspNetCore.Http;
using JGUZDV.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Th11s.TimeKeeping.Queries;
using Th11s.TimeKeeping.SharedModel.Primitives;
using Th11s.TimeKeeping.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JGUZDV.CQRS.Commands;
using Th11s.TimeKeeping.Commands;

namespace TimeKeeping.Web.Server.Endpoints
{
    public class Zeiterfassung
    {
        internal static async Task<IResult> Tagesansicht(
            Guid arbeitsplatzId,
            int jahr, int monat, int tag,
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

            var result = await queryHandler.ExecuteQuery(
                new LadeTagesdienstzeiten(arbeitsplatzId, datum),
                context.User, ct);

            return result.ToHttpResult();
        }

        internal static async Task<IResult> Monatsansicht(
            Guid arbeitsplatzId,
            int jahr, int monat,
            [FromServices] IQueryHandler<LadeMonatsdienstzeit> queryHandler,
            HttpContext context, CancellationToken ct)
        {
            var result = await queryHandler.ExecuteQuery(
                new LadeMonatsdienstzeit(arbeitsplatzId, jahr, monat),
                context.User, ct);

            return result.ToHttpResult();
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
            public required DateOnly Datum { get; init; }
            public required Stempeltyp Stempeltyp { get; init; }
            
            public DateTimeOffset? Zeitstempel { get; init; }
        }

        internal static async Task<IResult> ErfasseZeitstempel(
            Guid arbeitsplatzId,
            [FromBody] ZeitstempelRequest request,
            [FromServices] TimeProvider timeProvider,
            [FromServices] ICommandHandler<ErfasseStechzeit> commandHandler,
            HttpContext context, CancellationToken ct)
        {
            var result = await commandHandler.ExecuteAsync(
                new ErfasseStechzeit(arbeitsplatzId, request.Datum, request.Zeitstempel ?? timeProvider.GetUtcNow(), request.Stempeltyp),
                context.User, ct);

            return result.ToHttpResult();
        }
    }
}

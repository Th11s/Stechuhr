using System.Net;
using Th11s.TimeKeeping.SharedModel.Web;

namespace TimeKeeping.Web.Server.Endpoints
{
    public static class MinimalApiExtensions
    {
        public static void MapMinimalApi(this WebApplication app)
        {
            var api = app.MapGroup("/api/v1");
                api.MapGet("/{arbeitsplatzId:guid}/{jahr:int}-{monat:int}-{tag:int}", Zeiterfassung.Tagesansicht)
                    .Produces((int)HttpStatusCode.OK, typeof(Tagessicht))
                    .Produces((int)HttpStatusCode.BadRequest);

                api.MapGet("/{arbeitsplatzId:guid}/{jahr:int}-{monat:int}", Zeiterfassung.Monatsansicht)
                    .Produces((int)HttpStatusCode.OK, typeof(Monatssicht))
                    .Produces((int)HttpStatusCode.BadRequest);

                api.MapPost("/{arbeitsplatzId:guid}/", Zeiterfassung.ErfasseZeitstempel)
                    .Produces((int)HttpStatusCode.OK)
                    .Produces((int)HttpStatusCode.Created)
                    .Produces((int)HttpStatusCode.BadRequest);

            MapIntrospectionApi(api.MapGroup("/_app"));
            MapAdminApi(api.MapGroup("/admin"));
        }

        private static void MapIntrospectionApi(RouteGroupBuilder api)
        {
            api.MapGet("/user", Introspection.User)
                .Produces((int)HttpStatusCode.OK)
                .Produces((int)HttpStatusCode.Unauthorized)
                .WithName(nameof(Introspection.User));
        }

        private static void MapAdminApi(RouteGroupBuilder api)
        {

        }
    }
}

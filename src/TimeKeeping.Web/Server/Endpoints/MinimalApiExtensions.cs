using Th11s.TimeKeeping.SharedModel.Web;

namespace TimeKeeping.Web.Server.Endpoints
{
    public static class MinimalApiExtensions
    {
        public static void MapMinimalApi(this WebApplication app)
        {
            var api = app.MapGroup("/api/v1");
            api.MapGet("/{arbeitsplatzId:guid}/{jahr:int}-{monat:int}-{tag:int}", Zeiterfassung.Tagesansicht)
                .Produces(200, typeof(Tagessicht))
                .Produces(400);
        }
    }
}

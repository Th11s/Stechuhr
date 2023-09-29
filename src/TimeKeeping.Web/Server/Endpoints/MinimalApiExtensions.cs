using Th11s.TimeKeeping.SharedModel.Web;

namespace TimeKeeping.Web.Server.Endpoints
{
    public static class MinimalApiExtensions
    {
        public static void UseMinimalApi(this WebApplication app)
        {
            var api = app.MapGroup("/api/v1");
            api.MapGet("/{jahr}-{monat}-{tag}", Zeiterfassungsansichten.Tagesansicht)
                .Produces(200, typeof(Tagessicht))
                .Produces(400);
        }
    }
}

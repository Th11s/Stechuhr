
using Th11s.TimeKeeping.SharedModel.Web;

namespace TimeKeeping.Web.Server.Endpoints
{
    public static class Introspection
    {
        internal static IResult User(HttpContext context, CancellationToken ct)
        {
            if (!context.User.Identities.Any(x => x.IsAuthenticated))
                return Results.Unauthorized();

            return new AppUser(
                context.User.GetDisplayName(),
                context.User.Claims.Select(x => (x.Type, x.Value)).ToList()
                );
        }
    }
}

using System.Security.Claims;
using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.SharedModel.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal p)
            => p.FindFirst("sub")!.Value;

        public static string? GetDisplayName(this ClaimsPrincipal p) 
            => p.FindFirst("name")?.Value;

        public static string[] GetArbeitsplatzIds(this ClaimsPrincipal p)
            => p.FindAll(CustomClaimTypes.Arbeitsplatz).Select(x => x.Value).ToArray();
    }
}

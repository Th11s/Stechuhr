using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;
using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.Auth
{
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        private readonly ApplicationDbContext _dbContext;

        public UserClaimsPrincipalFactory(
            ApplicationDbContext dbContext,
            UserManager<User> userManager, 
            IOptions<IdentityOptions> optionsAccessor) 
            : base(userManager, optionsAccessor)
        {
            _dbContext = dbContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var arbeitsplaetze = await _dbContext.Users
                .Where(x => x.Id == user.Id)
                .SelectMany(x => x.Arbeitsplaetze!)
                .Select(x => x.Id)
                .ToListAsync();

            foreach (var arbeitsplatz in arbeitsplaetze)
                identity.AddClaim(new Claim(CustomClaimTypes.Arbeitsplatz, arbeitsplatz.ToString()));

            return identity;
        }
    }
}

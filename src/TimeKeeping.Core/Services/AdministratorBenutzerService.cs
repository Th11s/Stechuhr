using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Services
{
    public interface IAdminBenutzerService
    {
        Task ExecuteAsync(string password, CancellationToken ct);
    }

    internal class AdministratorBenutzerService(
        UserManager<User> _userManager,
        RoleManager<IdentityRole> _roleManager,
        ILogger<AdministratorBenutzerService> _logger
        ) : IAdminBenutzerService
    {
        public async Task ExecuteAsync(string password, CancellationToken ct)
        {
            var adminRole = await _roleManager.FindByNameAsync("admin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("admin");

                await _roleManager.CreateAsync(adminRole);
                adminRole = await _roleManager.FindByNameAsync("admin");
            }

            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin@localhost.tld",
                    Email = "admin@localhost.tld",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, password);
                if(!result.Succeeded)
                {
                    _logger.LogError(result.ToString());
                    return;
                }

                adminUser = await _userManager.FindByNameAsync(adminUser.UserName);
            }
            else
            {
                _logger.LogInformation("Adminaccount existiert, setze Passwort");
                var token = await _userManager.GeneratePasswordResetTokenAsync(adminUser);
                await _userManager.ResetPasswordAsync(adminUser, token, password);

                _logger.LogInformation("Passwort wurde zurückgesetzt");
            }

            await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }
    }
}

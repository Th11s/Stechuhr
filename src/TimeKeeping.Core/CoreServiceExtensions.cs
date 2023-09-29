using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Th11s.TimeKeeping.Data;

namespace Th11s.TimeKeeping
{
    public static class CoreServiceExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dbProvider = configuration.GetValue("DatabaseProvider", "Npgsql");
            switch (dbProvider)
            {
                case "Npgsql":
                    services.AddDbContext<NpgsqlDbContext>(
                    dbContext =>
                    {
                        dbContext.UseNpgsql(configuration.GetConnectionString("Npgsql"));
                    });

                    services.AddScoped<ApplicationDbContext, NpgsqlDbContext>();
                    break;

                case "SqlServer":
                    services.AddDbContext<SqlServerDbContext>(
                    dbContext =>
                    {
                        dbContext.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                    });

                    services.AddScoped<ApplicationDbContext, SqlServerDbContext>();
                    break;
            }

            services.AddTransient(sp => TimeProvider.System);
            services.AddCQRSHandlers(typeof(CoreServiceExtensions));

            return services;
        }
    }
}

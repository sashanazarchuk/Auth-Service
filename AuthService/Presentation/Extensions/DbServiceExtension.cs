using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions
{
    public static class DbServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("sqlConnection"));
            });
        }
    }
}

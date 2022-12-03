using API.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace API.DependencyInjections
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDatalayerServices(this IServiceCollection services, IConfiguration configuration){
            services.AddDbContext<FindmeContext>(options => {
                options.UseSqlite(configuration.GetConnectionString("FindmeSqliteConnection"));
            });
            return services;
        }
    }
}
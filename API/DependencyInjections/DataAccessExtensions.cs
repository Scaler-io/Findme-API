using API.DataAccess;
using API.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.DependencyInjections
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDatalayerServices(this IServiceCollection services, IConfiguration configuration){
            // database context
            services.AddDbContext<FindmeContext>(options => {
                options.UseSqlite(configuration.GetConnectionString("FindmeSqliteConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
using API.Services.Interfaces.v2;
using API.Services.v2.Account;
using API.Services.v2.Users;
using FluentValidation;
using System.Reflection;

namespace API.DependencyInjections
{
    public static class BusinessLayerExtensaions
    {
        public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

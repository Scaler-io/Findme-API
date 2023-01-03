using API.Models.Constants;

namespace API.DependencyInjections
{
    public static class HttpServiceExtensions
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddHttpClient(HttpClientNames.PostalValidationApi, c =>
            {
                c.BaseAddress = new Uri("https://api.postalpincode.in/");
                c.Timeout = new TimeSpan(0, 0, 25);
            });

            return services;
        } 
    }
}

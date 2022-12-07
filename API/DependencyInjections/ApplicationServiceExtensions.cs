using API.Infrastructure.Logger;
using API.Middlewares;
using API.Models.Core;
using API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

namespace API.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
            IConfiguration configuration
        )
        {
            // basics
            services.AddControllers();
            services.AddEndpointsApiExplorer()
                    .AddSwaggerGen();
            // serilog 
            var logger = LoggerConfig.Configure(configuration);
            services.AddSingleton(Log.Logger)   
                    .AddSingleton(x => logger);
            // http context accessor
            services.AddHttpContextAccessor();
            // api validation behaviours
            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = HandleFrameorkValidationFailure();
            });
            // api versioning
            services.AddApiVersioning(options => {
                options.DefaultApiVersion = new ApiVersion(1,0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            // swagger
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            // Cors
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            return services;
        }        

        public static WebApplication AddApplicationPipelines(this WebApplication app, IApiVersionDescriptionProvider provider)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options => { 
                    foreach(var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            // midlewares
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<RequestExceptionMiddleware>();

            app.UseCors("DefaultCorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            return app;
        }

        public static Func<ActionContext, IActionResult> HandleFrameorkValidationFailure(){
            return actionContext => {
                var errors = actionContext.ModelState
                            .Where(err => err.Value.Errors.Count > 0).ToList();
                var validationError = new ApiValidationResponse()
                {
                    Errors = new List<FieldLevelError>()
                };
                foreach(var error in errors){
                    var fieldLevelError = new FieldLevelError{
                        Code = "Invalid",
                        Field = error.Key,
                        Message = error.Value.Errors?.First().ErrorMessage
                    };
                    validationError.Errors.Add(fieldLevelError);
                }
                return new UnprocessableEntityObjectResult(validationError);
            };
        }
    }
}
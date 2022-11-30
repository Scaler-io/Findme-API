using API.Infrastructure;
using API.Middlewares;
using API.Models.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
            IConfiguration configuration
        ){
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
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            
            return services;
        }        

        public static WebApplication AddApplicationPipelines(this WebApplication app){
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // midlewares
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<RequestExceptionMiddleware>();

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
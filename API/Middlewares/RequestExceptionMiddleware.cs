using System.Net;
using System.Net.Mime;
using System.Text.Json;
using API.Extensions;
using API.Models.Core;
using ILogger = Serilog.ILogger;

namespace API.Middlewares
{
    public class RequestExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public RequestExceptionMiddleware(RequestDelegate next, ILogger logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }
            catch(Exception e){
                _logger.Here().Error(e, e.Message);
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                                 ? new ApiExceptionResponse(e.Message, e.StackTrace)
                                 : new ApiExceptionResponse(e.Message);

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
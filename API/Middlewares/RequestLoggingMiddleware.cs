using API.Extensions;
using Serilog;

namespace API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        public readonly RequestDelegate _next;
        public readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }finally{
                _logger.Here().Debug(
                    "Request {@method} {@url} => {@statusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode
                );
            }
        }
    }
}
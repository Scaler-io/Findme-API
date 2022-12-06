using API.Extensions;
using System.Net;
using ILogger = Serilog.ILogger;

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
                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.OK:
                        _logger.Here().Debug("Request {@method} {@url} => {@statusCode}",context.Request.Method,context.Request.Path,context.Response.StatusCode);
                        break;
                    case (int)HttpStatusCode.InternalServerError:
                        _logger.Here().Error("Request {@method} {@url} => {@statusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
                        break;
                    default:
                        _logger.Here().Warning("Request {@method} {@url} => {@statusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
                        break;
                }
            }
        }
    }
}
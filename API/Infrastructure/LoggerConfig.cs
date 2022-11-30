using Serilog;
using Serilog.Core;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace API.Infrastructure
{
    public class LoggerConfig
    {
        public static ILogger Configure(IConfiguration configuration){
            var loggerOptions = new LoggerConfigOptions();
            configuration.GetSection("LoggerConfigOption").Bind(loggerOptions);

            return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(new LoggingLevelSwitch(LogEventLevel.Debug))
                    .MinimumLevel.Override(loggerOptions.OverrideSource, Serilog.Events.LogEventLevel.Warning)
                    .WriteTo.Console(outputTemplate: loggerOptions.OutputTemplate)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                    .CreateLogger();
        }
    }
}
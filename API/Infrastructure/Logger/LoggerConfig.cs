using Destructurama;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using ILogger = Serilog.ILogger;

namespace API.Infrastructure.Logger
{
    public class LoggerConfig
    {
        public static ILogger Configure(IConfiguration configuration){
            var loggerOptions = new LoggerConfigOptions();
            configuration.GetSection("LoggerConfigOption").Bind(loggerOptions);

            return new LoggerConfiguration()
                    .Destructure
                    .UsingAttributes()
                    .Destructure
                    .With<LogDestructureModel>()
                    .MinimumLevel.ControlledBy(new LoggingLevelSwitch(LogEventLevel.Debug))
                    .MinimumLevel.Override(loggerOptions.OverrideSource, Serilog.Events.LogEventLevel.Warning)
                    .WriteTo.Console(outputTemplate: loggerOptions.OutputTemplate)
                    .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, loggerOptions.OutputTemplate))
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithMachineName()
                    .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string template)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return new ElasticsearchSinkOptions(new Uri(configuration["Elasticsearch:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"Findme-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}
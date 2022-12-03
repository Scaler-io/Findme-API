using API.DependencyInjections;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var host = builder.Host;

// services
services.AddApplicationServices(configuration)
        .AddDatalayerServices(configuration);

var app = builder.Build();
// pipelines
app.AddApplicationPipelines();

try{
        await app.RunAsync();
}finally{
        Log.CloseAndFlush();
}
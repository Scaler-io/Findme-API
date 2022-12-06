using API.DependencyInjections;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var host = builder.Host;

// enabling serilog
host.UseSerilog();


// services
services.AddApplicationServices(configuration)
        .AddDatalayerServices(configuration)
        .AddBusinessLayerServices();

var app = builder.Build();
var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// pipelines
app.AddApplicationPipelines(versionDescriptionProvider);

try{
        await app.RunAsync();
}finally{
        Log.CloseAndFlush();
}
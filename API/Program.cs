using API.DependencyInjections;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var host = builder.Host;
var app = builder.Build();

// services
services.AddApplicationServices(configuration);

// pipelines
app.AddApplicationPipelines();

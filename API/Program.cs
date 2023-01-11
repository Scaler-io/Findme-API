using API.DataAccess;
using API.DependencyInjections;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var host = builder.Host;

// enabling serilog
host.UseSerilog();

// services
services.AddApplicationServices(configuration)
        .AddDatalayerServices(configuration)
        .AddBusinessLayerServices()
        .AddHttpServices()
        .AddIdentityServices(configuration);

var app = builder.Build();
var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.AddApplicationPipelines(versionDescriptionProvider);

using (var scope = app.Services.CreateScope())
{
    var logger      = scope.ServiceProvider.GetRequiredService<ILogger>();
    var dbContext   = scope.ServiceProvider.GetRequiredService<FindmeContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    try
    {
        logger.Here().Information("{0}: Migration started", typeof(FindmeContext).Name);
        await dbContext.Database.MigrateAsync();
        logger.Here().Information("{0}: Migration completed", typeof(FindmeContext).Name);
        await FindmeContextSeed.SeedAsync(scope.ServiceProvider, logger, roleManager);
        await app.RunAsync();
    }
    catch(Exception e)
    {
        logger.Here().Error("{0}: Migration failed", typeof(FindmeContext).Name);
    }
    finally
    {
        Log.CloseAndFlush();
    }
}
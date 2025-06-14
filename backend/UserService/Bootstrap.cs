﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib.Bootstrap;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using UserService.Repository.Database;

namespace UserService;

/// <summary>
/// Handles dependency injection setup and manual AppSettings binding
/// </summary>
public static class Bootstrap
{
    /// <summary>
    /// Configures dependency injection and manual AppSettings binding
    /// </summary>
    /// <param name="builder">The web application builder</param>
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Logging.AddConsole();

        ConfigureAppSettings(builder);

        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .ToList();

        var configuration = new ContainerConfiguration()
            .WithAssemblies(assemblies);

        ConfigureCors(builder);
        ConfigureDependencyInjection(builder, assemblies, configuration);
        ConfigureDatabase(builder);
        ConfigureHttpClients(builder);
    }

    private static void ConfigureCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Authorization");
            });
        });
    }

    private static void ConfigureAppSettings(WebApplicationBuilder builder)
    {
        var appSettingsJson = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
        var appSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsJson);
        builder.Services.AddSingleton(typeof(AppSettings), appSettings!);
    }

    private static void ConfigureDependencyInjection(WebApplicationBuilder builder, List<Assembly> assemblies, ContainerConfiguration configuration)
    {
        foreach (var assembly in assemblies)
        {
            var exportedTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(ExportAttribute), inherit: false).Length > 0);

            foreach (var type in exportedTypes)
            {
                var exportAttributes = type.GetCustomAttributes<ExportAttribute>();

                foreach (var export in exportAttributes)
                {
                    var contractType = export.ContractType ?? type;

                    if (typeof(DatabaseContext).IsAssignableFrom(type) || contractType.Name.Contains("Repository"))
                    {
                        builder.Services.AddScoped(contractType, type);
                    }
                    else
                    {
                        builder.Services.AddSingleton(contractType, type);
                    }
                }
            }
        }
    }

    private static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();

        var appSettings = serviceProvider.GetService<AppSettings>();

        builder.Services.AddDbContext<DatabaseContext>
        (
            options =>
                options.UseMySql
                (
                    appSettings!.DBCongfigration.DefaultConnection,
                    new MySqlServerVersion(new Version(appSettings.DBCongfigration.MySqlVersion))
                )
        );
    }

    private static void ConfigureHttpClients(WebApplicationBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();

        var appSettings = serviceProvider.GetService<AppSettings>();

        if (appSettings?.HttpClients is null || !appSettings.HttpClients.Any())
        {
            return;
        }

        foreach (var httpClientConfig in appSettings.HttpClients)
        {
            builder.Services.AddHttpClient
            (
                httpClientConfig.Name,
                config =>
                {
                    config.BaseAddress = new Uri($"{httpClientConfig.ServiceBaseUrl}");
                }
            );
        }
    }
}

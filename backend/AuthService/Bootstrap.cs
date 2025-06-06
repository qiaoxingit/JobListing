using Newtonsoft.Json;
using SharedLib.Bootstrap;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;

namespace AuthService;

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
        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .ToList();

        var configuration = new ContainerConfiguration()
            .WithAssemblies(assemblies);

        var mefContainer = configuration.CreateContainer();

        var appSettings = mefContainer.GetExport<AppSettings>();
        var appSettingsJson = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
        JsonConvert.PopulateObject(appSettingsJson, appSettings);

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

                    var instance = mefContainer.GetExport(contractType);

                    builder.Services.AddSingleton(contractType, instance);
                }
            }
        }
    }
}

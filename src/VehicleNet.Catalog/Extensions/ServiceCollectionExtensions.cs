using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.DependencyInjection;
using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Catalog.Services;
using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Catalog.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVehicleCatalogServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var assembly = typeof(JsonVehicleCatalogSource).Assembly;

        var manufacturers = LoadResource(assembly, "manufacturers.json", context => context.IReadOnlyListManufacturerDto);
        var models = LoadResource(assembly, "models.json", context => context.IReadOnlyListModelDto);
        var generations = LoadResource(assembly, "generations.json", context => context.IReadOnlyListGenerationDto);
        var versions = LoadResource(assembly, "versions.json", context => context.IReadOnlyListVersionDto);
        var engines = LoadResource(assembly, "engines.json", context => context.IReadOnlyListEngineDto);

        services.AddSingleton<IEnumerable<ManufacturerDto>>(manufacturers);
        services.AddSingleton<IEnumerable<ModelDto>>(models);
        services.AddSingleton<IEnumerable<GenerationDto>>(generations);
        services.AddSingleton<IEnumerable<VersionDto>>(versions);
        services.AddSingleton<IEnumerable<EngineDto>>(engines);

        services.AddSingleton<IManufacturerService, ManufacturerService>();
        services.AddSingleton<IModelService, ModelService>();
        services.AddSingleton<IGenerationService, GenerationService>();
        services.AddSingleton<IVersionService, VersionService>();
        services.AddSingleton<IEngineService, EngineService>();

        services.AddSingleton<IVehicleCatalogSource, JsonVehicleCatalogSource>();

        services.AddSingleton<IEnumerable<VehicleSpec>>(sp =>
            sp.GetRequiredService<IVehicleCatalogSource>()
                .LoadAsync()
                .GetAwaiter()
                .GetResult());

        services.AddSingleton<IEnumerable<VehicleBody>>(sp =>
            sp.GetRequiredService<IEnumerable<VehicleSpec>>()
                .Select(v => v.VehicleBody)
                .OfType<VehicleBody>()
                .DistinctBy(b => b.VehicleBodyId)
                .ToList());

        services.AddSingleton<IVehicleSpecService, VehicleSpecService>();
        services.AddSingleton<IVehicleBodyService, VehicleBodyService>();

        return services;
    }

    private static IReadOnlyList<T> LoadResource<T>(
        Assembly assembly,
        string fileName,
        Func<CatalogJsonSerializerContext, JsonTypeInfo<IReadOnlyList<T>>> jsonTypeInfoFactory)
    {
        var resourceName = assembly
            .GetManifestResourceNames()
            .SingleOrDefault(name => name.EndsWith($"Data.{fileName}", StringComparison.Ordinal))
            ?? throw new InvalidOperationException($"Embedded catalog JSON resource 'Data/{fileName}' was not found.");

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Catalog resource '{resourceName}' was not found.");

        var result = JsonSerializer.Deserialize(stream, jsonTypeInfoFactory(CatalogJsonSerializerContext.Default));

        return result ?? throw new InvalidOperationException($"Catalog JSON resource '{fileName}' was empty or invalid.");
    }
}

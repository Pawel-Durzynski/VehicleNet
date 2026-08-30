using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.DependencyInjection;
using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Catalog.Services;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Search;
using VehicleNet.Common.Models.Units;

namespace VehicleNet.Catalog.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVehicleCatalogServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var assembly = typeof(JsonVehicleCatalogSource).Assembly;

        var manufacturers = LoadResource(assembly, "1-manufacturers.json", context => context.IReadOnlyListManufacturerDto);
        var models = LoadResource(assembly, "2-models.json", context => context.IReadOnlyListModelDto);
        var generations = LoadResource(assembly, "3-generations.json", context => context.IReadOnlyListGenerationDto);
        var versions = LoadResource(assembly, "4-versions.json", context => context.IReadOnlyListVersionDto);
        var engines = LoadResource(assembly, "5-engines.json", context => context.IReadOnlyListEngineDto);
        var engineVariants = LoadResource(assembly, "6-engine-variants.json", context => context.IReadOnlyListEngineVariantDto);

        // Validate ID uniqueness
        ValidateUniqueIds(manufacturers, m => m.Id, "Manufacturer", "1-manufacturers.json");
        ValidateUniqueIds(models, m => m.Id, "Model", "2-models.json");
        ValidateUniqueIds(generations, g => g.Id, "Generation", "3-generations.json");
        ValidateUniqueIds(versions, v => v.Id, "Version", "4-versions.json");
        ValidateUniqueIds(engines, e => e.Id, "Engine", "5-engines.json");
        ValidateUniqueIds(engineVariants, ev => ev.EngineVariantId, "EngineVariant", "6-engine-variants.json");

        services.AddSingleton<IEnumerable<ManufacturerDto>>(manufacturers);
        services.AddSingleton<IEnumerable<ModelDto>>(models);
        services.AddSingleton<IEnumerable<GenerationDto>>(generations);
        services.AddSingleton<IEnumerable<VersionDto>>(versions);
        services.AddSingleton<IEnumerable<EngineDto>>(engines);
        services.AddSingleton<IEnumerable<EngineVariantDto>>(engineVariants);

        services.AddSingleton<IManufacturerService, ManufacturerService>();
        services.AddSingleton<IModelService, ModelService>();
        services.AddSingleton<IGenerationService, GenerationService>();
        services.AddSingleton<IVersionService, VersionService>();
        services.AddSingleton<IEngineService, EngineService>();
        services.AddSingleton<IEngineVariantService, EngineVariantService>();

        services.AddSingleton<IVehicleCatalogSource, JsonVehicleCatalogSource>();

        services.AddSingleton<IEnumerable<VehicleBodyEngine>>(sp =>
            sp.GetRequiredService<IVehicleCatalogSource>()
                .LoadAsync()
                .GetAwaiter()
                .GetResult());

        services.AddSingleton<IEnumerable<VehicleBody>>(sp =>
            sp.GetRequiredService<IEnumerable<VehicleBodyEngine>>()
                .Select(v => v.VehicleBody)
                .OfType<VehicleBody>()
                .DistinctBy(b => b.VehicleBodyId)
                .ToList());

        services.AddSingleton<IVehicleBodyEngineService, VehicleBodyEngineService>();
        services.AddSingleton<IVehicleBodyService, VehicleBodyService>();

        // Load and convert vehicle body engine variants
        var vehicleBodyEngineVariantDtos = LoadResource(assembly, "9-vehicle-body-engine-variants.json", context => context.IReadOnlyListVehicleBodyEngineVariantDto);
        ValidateUniqueIds(vehicleBodyEngineVariantDtos, vbev => vbev.VehicleBodyEngineVariantId, "VehicleBodyEngineVariant", "9-vehicle-body-engine-variants.json");

        services.AddSingleton<IEnumerable<VehicleBodyEngineVariant>>(sp =>
        {
            var bodyEngineService = sp.GetRequiredService<IVehicleBodyEngineService>();
            var engineVariantService = sp.GetRequiredService<IEngineVariantService>();
            return vehicleBodyEngineVariantDtos
                .Select(dto => MapVehicleBodyEngineVariant(dto, bodyEngineService, engineVariantService))
                .ToList();
        });

        services.AddSingleton<IVehicleBodyEngineVariantService, VehicleBodyEngineVariantService>();

        return services;
    }

    private static VehicleBodyEngineVariant MapVehicleBodyEngineVariant(
        VehicleBodyEngineVariantDto dto,
        IVehicleBodyEngineService vehicleBodyEngineService,
        IEngineVariantService engineVariantService)
    {
        var bodyEngineCriteria = new VehicleBodyEngineSearchCriteriaBuilder()
            .WithVehicleBodyEngineId(dto.VehicleBodyEngineId)
            .Build();

        var bodyEngineResult = vehicleBodyEngineService.Search(bodyEngineCriteria);
        var bodyEngine = bodyEngineResult.Items.FirstOrDefault()
            ?? throw new InvalidOperationException($"VehicleBodyEngine {dto.VehicleBodyEngineId} was not found for variant {dto.VehicleBodyEngineVariantId}.");

        var engineVariants = engineVariantService.Search(new EngineVariantSearch { });
        var engineVariant = engineVariants.FirstOrDefault(v => v.EngineVariantId == dto.EngineVariantId)
            ?? throw new InvalidOperationException($"EngineVariant {dto.EngineVariantId} was not found for variant {dto.VehicleBodyEngineVariantId}.");

        return new VehicleBodyEngineVariant
        {
            VehicleBodyEngineVariantId = dto.VehicleBodyEngineVariantId,
            GenerationId = bodyEngine.GenerationId,
            VersionId = bodyEngine.VersionId,
            VehicleBodyEngineId = dto.VehicleBodyEngineId,
            EngineVariantId = dto.EngineVariantId,
            EngineVariantSpecs = new EngineVariantSpecs
            {
                DrivetrainSpecs = dto.EngineVariantSpecs.DrivetrainSpecs is null
                    ? null
                    : new DrivetrainSpecs
                    {
                        TransmissionType = dto.EngineVariantSpecs.DrivetrainSpecs.TransmissionType,
                        Drivetrain = dto.EngineVariantSpecs.DrivetrainSpecs.Drivetrain
                    },
                PerformanceSpecs = dto.EngineVariantSpecs.PerformanceSpecs is null
                    ? null
                    : new PerformanceSpecs
                    {
                        Acceleration0To100 = dto.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100 is not null
                            ? new ParameterValue(
                                dto.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100.Value,
                                dto.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100.Unit,
                                dto.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100.IsMissing)
                            : ParameterValue.Missing(MeasurementUnit.Second),
                        TopSpeed = dto.EngineVariantSpecs.PerformanceSpecs.TopSpeed is not null
                            ? new ParameterValue(
                                dto.EngineVariantSpecs.PerformanceSpecs.TopSpeed.Value,
                                dto.EngineVariantSpecs.PerformanceSpecs.TopSpeed.Unit,
                                dto.EngineVariantSpecs.PerformanceSpecs.TopSpeed.IsMissing)
                            : ParameterValue.Missing(MeasurementUnit.KilometerPerHour)
                    }
            },
            VehicleBodyEngine = bodyEngine,
            Generation = bodyEngine.Generation,
            Version = bodyEngine.Version,
            EngineVariant = engineVariant
        };
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

    private static void ValidateUniqueIds<T>(IReadOnlyList<T> items, Func<T, int> idSelector, string entityName, string fileName)
    {
        var duplicates = items
            .GroupBy(idSelector)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Count > 0)
        {
            throw new InvalidOperationException(
                $"Duplicate {entityName} IDs found in '{fileName}': {string.Join(", ", duplicates)}. Each {entityName} must have a unique ID.");
        }
    }
}

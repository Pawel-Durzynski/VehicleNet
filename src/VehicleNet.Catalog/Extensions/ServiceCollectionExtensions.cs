using System.Globalization;
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
        ValidateUniqueIds(engineVariants, ev => ev.Id, "EngineVariant", "6-engine-variants.json");

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
        ValidateUniqueIds(vehicleBodyEngineVariantDtos, vbev => vbev.Id, "VehicleBodyEngineVariant", "9-vehicle-body-engine-variants.json");

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
            .WithVehicleBodyEngineId(dto.VbeId)
            .Build();

        var bodyEngineResult = vehicleBodyEngineService.Search(bodyEngineCriteria);
        var bodyEngine = bodyEngineResult.Items.FirstOrDefault()
            ?? throw new InvalidOperationException($"VehicleBodyEngine {dto.VbeId} was not found for variant {dto.Id}.");

        var engineVariants = engineVariantService.Search(new EngineVariantSearch { });
        var engineVariant = engineVariants.FirstOrDefault(v => v.EngineVariantId == dto.EvId)
            ?? throw new InvalidOperationException($"EngineVariant {dto.EvId} was not found for variant {dto.Id}.");

        return new VehicleBodyEngineVariant
        {
            VehicleBodyEngineVariantId = dto.Id,
            GenerationId = bodyEngine.GenerationId,
            VersionId = bodyEngine.VersionId,
            VehicleBodyEngineId = dto.VbeId,
            EngineVariantId = dto.EvId,
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
                        Acceleration0To100 = ToParameterValue(dto.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100, MeasurementUnit.Second),
                        TopSpeed = ToParameterValue(dto.EngineVariantSpecs.PerformanceSpecs.TopSpeed, MeasurementUnit.KilometerPerHour)
                    }
            },
            VehicleBodyEngine = bodyEngine,
            Generation = bodyEngine.Generation,
            Version = bodyEngine.Version,
            EngineVariant = engineVariant
        };
    }

    private static ParameterValue ToParameterValue(JsonElement? element, MeasurementUnit defaultUnit)
    {
        if (!element.HasValue)
        {
            return ParameterValue.Missing(defaultUnit);
        }

        var value = element.Value;

        if (value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return ParameterValue.Missing(defaultUnit);
        }

        if (value.ValueKind is JsonValueKind.Number)
        {
            return ParameterValue.Create(value.GetDecimal(), defaultUnit);
        }

        if (value.ValueKind is JsonValueKind.String)
        {
            var raw = value.GetString();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return ParameterValue.Missing(defaultUnit);
            }

            if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedValue))
            {
                throw new InvalidOperationException($"Could not parse parameter value '{raw}'.");
            }

            return ParameterValue.Create(parsedValue, defaultUnit);
        }

        throw new InvalidOperationException($"Unsupported parameter value token kind '{value.ValueKind}'.");
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

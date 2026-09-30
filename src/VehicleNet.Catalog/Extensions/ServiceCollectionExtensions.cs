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
        var document = new CatalogJsonDocument
        {
            Manufacturers = LoadResource(assembly, "1-manufacturers.json", context => context.IReadOnlyListManufacturerDto),
            Models = LoadResource(assembly, "2-models.json", context => context.IReadOnlyListModelDto),
            Generations = LoadResource(assembly, "3-generations.json", context => context.IReadOnlyListGenerationDto),
            Versions = LoadResource(assembly, "4-versions.json", context => context.IReadOnlyListVersionDto),
            Engines = LoadResource(assembly, "5-engines.json", context => context.IReadOnlyListEngineDto),
            EngineVariants = LoadResource(assembly, "6-engine-variants.json", context => context.IReadOnlyListEngineVariantDto),
            VehicleBodies = LoadResource(assembly, "7-vehicle-bodies.json", context => context.IReadOnlyListVehicleBodyDto),
            VehicleBodyEngines = LoadResource(assembly, "8-vehicle-body-engines.json", context => context.IReadOnlyListVehicleBodyEngineDto),
            VehicleBodyEngineVariants = LoadResource(assembly, "9-vehicle-body-engine-variants.json", context => context.IReadOnlyListVehicleBodyEngineVariantDto)
        };
        var vehicleBodyEngines = new CatalogSnapshotBuilder().Build(document);

        services.AddSingleton<IEnumerable<ManufacturerDto>>(document.Manufacturers);
        services.AddSingleton<IEnumerable<ModelDto>>(document.Models);
        services.AddSingleton<IEnumerable<GenerationDto>>(document.Generations);
        services.AddSingleton<IEnumerable<VersionDto>>(document.Versions);
        services.AddSingleton<IEnumerable<EngineDto>>(document.Engines);
        services.AddSingleton<IEnumerable<EngineVariantDto>>(document.EngineVariants);

        services.AddSingleton<IManufacturerService, ManufacturerService>();
        services.AddSingleton<IModelService, ModelService>();
        services.AddSingleton<IGenerationService, GenerationService>();
        services.AddSingleton<IVersionService, VersionService>();
        services.AddSingleton<IEngineService, EngineService>();
        services.AddSingleton<IEngineVariantService, EngineVariantService>();

        services.AddSingleton<IVehicleCatalogSource>(new JsonVehicleCatalogSource(vehicleBodyEngines));
        services.AddSingleton<IEnumerable<VehicleBodyEngine>>(vehicleBodyEngines);

        services.AddSingleton<IEnumerable<VehicleBody>>(sp =>
            sp.GetRequiredService<IEnumerable<VehicleBodyEngine>>()
                .Select(v => v.VehicleBody)
                .OfType<VehicleBody>()
                .DistinctBy(b => b.VehicleBodyId)
                .ToList());

        services.AddSingleton<IVehicleBodyEngineService, VehicleBodyEngineService>();
        services.AddSingleton<IVehicleBodyService, VehicleBodyService>();

        services.AddSingleton<IEnumerable<VehicleBodyEngineVariant>>(sp =>
        {
            var bodyEnginesById = sp
                .GetRequiredService<IEnumerable<VehicleBodyEngine>>()
                .ToDictionary(bodyEngine => bodyEngine.VehicleBodyEngineId);
            var engineVariantsById = sp
                .GetRequiredService<IEngineVariantService>()
                .Search(new EngineVariantSearch { })
                .ToDictionary(engineVariant => engineVariant.EngineVariantId);

            return document.VehicleBodyEngineVariants
                .Select(dto => MapVehicleBodyEngineVariant(dto, bodyEnginesById, engineVariantsById))
                .ToList();
        });

        services.AddSingleton<IVehicleBodyEngineVariantService, VehicleBodyEngineVariantService>();

        return services;
    }

    private static VehicleBodyEngineVariant MapVehicleBodyEngineVariant(
        VehicleBodyEngineVariantDto dto,
        IReadOnlyDictionary<int, VehicleBodyEngine> bodyEnginesById,
        IReadOnlyDictionary<int, EngineVariant> engineVariantsById)
    {
        if (!bodyEnginesById.TryGetValue(dto.VbeId, out var bodyEngine))
        {
            throw new InvalidOperationException($"VehicleBodyEngine {dto.VbeId} was not found for variant {dto.Id}.");
        }

        if (!engineVariantsById.TryGetValue(dto.EvId, out var engineVariant))
        {
            throw new InvalidOperationException($"EngineVariant {dto.EvId} was not found for variant {dto.Id}.");
        }

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
}

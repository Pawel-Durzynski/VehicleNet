using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Catalog.BodyDetails;
using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Units;

namespace VehicleNet.Catalog.Data.Json;

internal sealed class CatalogSnapshotBuilder
{
    public IReadOnlyList<VehicleBodyEngine> Build(CatalogJsonDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var manufacturers = CreateIndex(
            document.Manufacturers,
            manufacturer => manufacturer.Id,
            manufacturer => new VehicleManufacturer(
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.Manufacturer),
            "manufacturer");

        var models = CreateIndex(
            document.Models,
            model => model.Id,
            model => new VehicleModel(
                model.Id,
                model.Name,
                model.ManufacturerId,
                GetRequired(manufacturers, model.ManufacturerId, "manufacturer", $"model {model.Id}")),
            "model");

        var generations = CreateIndex(
            document.Generations,
            generation => generation.Id,
            generation => new VehicleGeneration(
                generation.Id,
                generation.ModelId,
                generation.Name,
                generation.StartYear,
                generation.EndYear,
                GetRequired(models, generation.ModelId, "model", $"generation {generation.Id}"),
                generation.ContainsVersions),
            "generation");

        var versions = CreateIndex(
            document.Versions,
            version => version.Id,
            version => new VehicleVersion(
                version.Id,
                version.GenerationId,
                version.Name,
                version.StartYear,
                version.EndYear,
                version.BodyType,
                GetRequired(generations, version.GenerationId, "generation", $"version {version.Id}")),
            "version");

        var engines = CreateIndex(
            document.Engines,
            engine => engine.Id,
            engine => new VehicleEngine(
                engine.Id,
                engine.Name,
                engine.GenerationId,
                engine.GenerationId.HasValue ? GetRequired(generations, engine.GenerationId.Value, "generation", $"engine {engine.Id}") : null,
                engine.VersionId,
                engine.VersionId.HasValue ? GetRequired(versions, engine.VersionId.Value, "version", $"engine {engine.Id}") : null),
            "engine");

        var bodies = CreateIndex(
            document.VehicleBodies,
            body => body.VehicleBodyId,
            body => new VehicleBody
            {
                VehicleBodyId = body.VehicleBodyId,
                GenerationId = body.GenerationId,
                Generation = body.GenerationId.HasValue ? GetRequired(generations, body.GenerationId.Value, "generation", $"body {body.VehicleBodyId}") : null,
                VersionId = body.VersionId,
                Version = body.VersionId.HasValue ? GetRequired(versions, body.VersionId.Value, "version", $"body {body.VehicleBodyId}") : null,
                BodySpecs = BuildBodySpecs(body.BodySpecs)
            },
            "body");

        return document.VehicleBodyEngines
            .Select(spec => BuildVehicleBodyEngine(spec, bodies, generations, versions, engines))
            .ToArray();
    }

    private static VehicleBodyEngine BuildVehicleBodyEngine(
        VehicleBodyEngineDto bodyEngine,
        IReadOnlyDictionary<int, VehicleBody> bodies,
        IReadOnlyDictionary<int, VehicleGeneration> generations,
        IReadOnlyDictionary<int, VehicleVersion> versions,
        IReadOnlyDictionary<int, VehicleEngine> engines)
    {
        var engineId = bodyEngine.EngineId ?? throw new InvalidOperationException($"vehicle spec {bodyEngine.VehicleBodyEngineId} is missing engineId.");
        var body = GetRequired(bodies, bodyEngine.VehicleBodyId, "body", $"vehicle spec {bodyEngine.VehicleBodyEngineId}");
        var generationId = body.GenerationId ?? throw new InvalidOperationException($"body {body.VehicleBodyId} is missing generationId.");
        var versionId = body.VersionId ?? throw new InvalidOperationException($"body {body.VehicleBodyId} is missing versionId.");

        return new VehicleBodyEngine
        {
            VehicleBodyEngineId = bodyEngine.VehicleBodyEngineId,
            VehicleBodyId = bodyEngine.VehicleBodyId,
            VehicleBody = body,
            GenerationId = generationId,
            Generation = GetRequired(generations, generationId, "generation", $"vehicle spec {bodyEngine.VehicleBodyEngineId}"),
            VersionId = versionId,
            Version = GetRequired(versions, versionId, "version", $"vehicle spec {bodyEngine.VehicleBodyEngineId}"),
            EngineId = engineId,
            Engine = GetRequired(engines, engineId, "engine", $"vehicle spec {bodyEngine.VehicleBodyEngineId}"),
            EngineSpecs = BuildEngineSpecs(bodyEngine.EngineSpecs)
        };
    }

    private static BodySpecs BuildBodySpecs(BodySpecsDto dto)
    {
        dto ??= new BodySpecsDto();

        return new BodySpecs
        {
            BasicParameters = dto.BasicParameters is null
                ? null
                : new BasicParameters
                {
                    NumberOfDoors = ToParameterValue(dto.BasicParameters.NumberOfDoors, MeasurementUnit.Count),
                    NumberOfSeats = ToParameterValue(dto.BasicParameters.NumberOfSeats, MeasurementUnit.Count),
                    TurningDiameter = ToParameterValue(dto.BasicParameters.TurningDiameter, MeasurementUnit.Meter),
                    TurningRadius = ToParameterValue(dto.BasicParameters.TurningRadius, MeasurementUnit.Meter),
                },
            ExternalDimensions = dto.ExternalDimensions is null
                ? null
                : new ExternalDimensions
                {
                    Length = ToParameterValue(dto.ExternalDimensions.Length, MeasurementUnit.Millimeter),
                    Width = ToParameterValue(dto.ExternalDimensions.Width, MeasurementUnit.Millimeter),
                    Height = ToParameterValue(dto.ExternalDimensions.Height, MeasurementUnit.Millimeter),
                    Wheelbase = ToParameterValue(dto.ExternalDimensions.Wheelbase, MeasurementUnit.Millimeter),
                    GroundClearance = ToParameterValue(dto.ExternalDimensions.GroundClearance, MeasurementUnit.Millimeter),
                },
            TrunkDimensions = dto.TrunkDimensions is null
                ? null
                : new TrunkDimensions
                {
                    MaximumTrunkCapacitySeatsFolded = ToParameterValue(dto.TrunkDimensions.MaximumTrunkCapacitySeatsFolded, MeasurementUnit.Liter),
                    MinimumTrunkCapacitySeatsUp = ToParameterValue(dto.TrunkDimensions.MinimumTrunkCapacitySeatsUp, MeasurementUnit.Liter),
                }
        };
    }

    private static EngineSpecs BuildEngineSpecs(EngineSpecsDto dto)
    {
        dto ??= new EngineSpecsDto();

        return new EngineSpecs
        {
            Capacity = ToParameterValue(dto.Capacity, MeasurementUnit.CubicCentimeter),
            FuelType = dto.FuelType,
            Architecture = dto.Architecture is null
                ? null
                : new EngineArchitecture
                {
                    CylinderCount = ToParameterValue(dto.Architecture.CylinderCount, MeasurementUnit.Count),
                    CylinderArrangement = dto.Architecture.CylinderArrangement,
                    ValveCount = ToParameterValue(dto.Architecture.ValveCount, MeasurementUnit.Count)
                },
            Power = dto.Power is null
                ? null
                : new EnginePowerSpecs
                {
                    Horsepower = ToParameterValue(dto.Power.Horsepower, MeasurementUnit.Horsepower),
                    AtRpm = ToParameterValue(dto.Power.AtRpm, MeasurementUnit.Rpm)
                },
            Torque = dto.Torque is null
                ? null
                : new EngineTorqueSpecs
                {
                    MaxTorque = ToParameterValue(dto.Torque.MaxTorque, MeasurementUnit.NewtonMeter),
                    AtRpmFrom = ToParameterValue(dto.Torque.AtRpmFrom, MeasurementUnit.Rpm),
                    AtRpmTo = ToParameterValue(dto.Torque.AtRpmTo, MeasurementUnit.Rpm)
                }
        };
    }

    private static ParameterValue ToParameterValue(ParameterValueDto? dto, MeasurementUnit defaultUnit)
    {
        if (dto is null)
        {
            return ParameterValue.Missing(defaultUnit);
        }

        return dto.IsMissing || !dto.Value.HasValue
            ? ParameterValue.Missing(dto.Unit == MeasurementUnit.None ? defaultUnit : dto.Unit)
            : ParameterValue.Create(dto.Value.Value, dto.Unit == MeasurementUnit.None ? defaultUnit : dto.Unit);
    }

    private static TTarget GetRequired<TTarget>(IReadOnlyDictionary<int, TTarget> items, int id, string itemName, string owner)
        where TTarget : class
    {
        if (items.TryGetValue(id, out var item))
        {
            return item;
        }

        throw new InvalidOperationException($"{owner} references missing {itemName} {id}.");
    }

    private static IReadOnlyDictionary<int, TTarget> CreateIndex<TSource, TTarget>(
        IEnumerable<TSource> items,
        Func<TSource, int> keySelector,
        Func<TSource, TTarget> valueSelector,
        string itemName)
        where TTarget : class
    {
        var index = new Dictionary<int, TTarget>();

        foreach (var item in items)
        {
            var key = keySelector(item);
            if (!index.TryAdd(key, valueSelector(item)))
            {
                throw new InvalidOperationException($"Duplicate {itemName} id '{key}' found in catalog JSON.");
            }
        }

        return index;
    }
}

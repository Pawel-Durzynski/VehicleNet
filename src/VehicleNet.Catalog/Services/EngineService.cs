using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class EngineService : IEngineService
{
    private readonly IReadOnlyList<EngineDto> _engines;
    private readonly IReadOnlyDictionary<int, GenerationDto> _generationsById;
    private readonly IReadOnlyDictionary<int, VersionDto> _versionsById;
    private readonly IReadOnlyDictionary<int, ModelDto> _modelsById;
    private readonly IReadOnlyDictionary<int, ManufacturerDto> _manufacturersById;

    public EngineService(
        IEnumerable<EngineDto> engines,
        IEnumerable<GenerationDto> generations,
        IEnumerable<VersionDto> versions,
        IEnumerable<ModelDto> models,
        IEnumerable<ManufacturerDto> manufacturers)
    {
        _engines = engines.ToList();
        _generationsById = generations.ToDictionary(generation => generation.Id);
        _versionsById = versions.ToDictionary(version => version.Id);
        _modelsById = models.ToDictionary(model => model.Id);
        _manufacturersById = manufacturers.ToDictionary(manufacturer => manufacturer.Id);
    }

    public IEnumerable<VehicleEngine> Search(EngineSearch search)
    {
        ArgumentNullException.ThrowIfNull(search);

        var query = _engines.AsEnumerable();

        if (search.GenerationId.HasValue)
        {
            query = query.Where(engine => engine.GenerationId == search.GenerationId.Value);
        }

        if (search.VersionId.HasValue)
        {
            query = query.Where(engine => engine.VersionId == search.VersionId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.Name))
        {
            query = query.Where(engine =>
                engine.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
        }

        return query
            .Select(MapEngine)
            .OrderBy(engine => engine.Name)
            .ToList();
    }

    private VehicleEngine MapEngine(EngineDto engine)
    {
        var version = engine.VersionId.HasValue
            ? BuildVersion(engine.VersionId.Value)
            : null;

        var generation = engine.GenerationId.HasValue
            ? BuildGeneration(engine.GenerationId.Value)
            : version?.Generation;

        return new VehicleEngine(
            engine.Id,
            engine.Name,
            generation?.Id,
            generation,
            version?.Id,
            version);
    }

    private VehicleVersion BuildVersion(int versionId)
    {
        if (!_versionsById.TryGetValue(versionId, out var version))
        {
            throw new InvalidOperationException($"version {versionId} was not found for engine.");
        }

        var generation = BuildGeneration(version.GenerationId);

        return new VehicleVersion(
            version.Id,
            version.GenerationId,
            version.Name,
            version.StartYear,
            version.EndYear,
            version.BodyType,
            generation);
    }

    private VehicleGeneration BuildGeneration(int generationId)
    {
        if (!_generationsById.TryGetValue(generationId, out var generation))
        {
            throw new InvalidOperationException($"generation {generationId} was not found for engine.");
        }

        if (!_modelsById.TryGetValue(generation.ModelId, out var model))
        {
            throw new InvalidOperationException($"model {generation.ModelId} was not found for engine.");
        }

        if (!_manufacturersById.TryGetValue(model.ManufacturerId, out var manufacturer))
        {
            throw new InvalidOperationException($"manufacturer {model.ManufacturerId} was not found for engine.");
        }

        var vehicleManufacturer = new VehicleManufacturer(
            manufacturer.Id,
            manufacturer.Name,
            manufacturer.Manufacturer);

        var vehicleModel = new VehicleModel(
            model.Id,
            model.Name,
            model.ManufacturerId,
            vehicleManufacturer);

        return new VehicleGeneration(
            generation.Id,
            generation.ModelId,
            generation.Name,
            generation.StartYear,
            generation.EndYear,
            vehicleModel,
            generation.ContainsVersions);
    }
}

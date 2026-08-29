using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class VersionService : IVersionService
{
    private readonly IReadOnlyList<VersionDto> _versions;
    private readonly IReadOnlyDictionary<int, GenerationDto> _generationsById;
    private readonly IReadOnlyDictionary<int, ModelDto> _modelsById;
    private readonly IReadOnlyDictionary<int, ManufacturerDto> _manufacturersById;

    public VersionService(
        IEnumerable<VersionDto> versions,
        IEnumerable<GenerationDto> generations,
        IEnumerable<ModelDto> models,
        IEnumerable<ManufacturerDto> manufacturers)
    {
        _versions = versions.ToList();
        _generationsById = generations.ToDictionary(generation => generation.Id);
        _modelsById = models.ToDictionary(model => model.Id);
        _manufacturersById = manufacturers.ToDictionary(manufacturer => manufacturer.Id);
    }

    public IEnumerable<VehicleVersion> Search(VersionSearch search)
    {
        ArgumentNullException.ThrowIfNull(search);

        var query = _versions.AsEnumerable();

        if (search.GenerationId.HasValue)
        {
            query = query.Where(version => version.GenerationId == search.GenerationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.Name))
        {
            query = query.Where(version =>
                version.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
        }

        return query
            .Select(version =>
            {
                if (!_generationsById.TryGetValue(version.GenerationId, out var generation))
                {
                    throw new InvalidOperationException($"generation {version.GenerationId} was not found for version.");
                }

                if (!_modelsById.TryGetValue(generation.ModelId, out var model))
                {
                    throw new InvalidOperationException($"model {generation.ModelId} was not found for version.");
                }

                if (!_manufacturersById.TryGetValue(model.ManufacturerId, out var manufacturer))
                {
                    throw new InvalidOperationException($"manufacturer {model.ManufacturerId} was not found for version.");
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

                var vehicleGeneration = new VehicleGeneration(
                    generation.Id,
                    generation.ModelId,
                    generation.Name,
                    generation.StartYear,
                    generation.EndYear,
                    vehicleModel,
                    generation.ContainsVersions);

                return new VehicleVersion(
                    version.Id,
                    version.GenerationId,
                    version.Name,
                    version.StartYear,
                    version.EndYear,
                    version.BodyType,
                    vehicleGeneration);
            })
            .OrderBy(version => version.Name)
            .ToList();
    }
}

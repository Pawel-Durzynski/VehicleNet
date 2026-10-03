using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class GenerationService : IGenerationService
{
    private readonly IReadOnlyList<GenerationDto> _generations;
    private readonly IReadOnlyDictionary<int, ModelDto> _modelsById;
    private readonly IReadOnlyDictionary<int, ManufacturerDto> _manufacturersById;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerationService"/> class with the specified data sources.
    /// </summary>
    /// <param name="generations">The collection of generation data transfer objects.</param>
    /// <param name="models">The collection of model data transfer objects.</param>
    /// <param name="manufacturers">The collection of manufacturer data transfer objects.</param>
    public GenerationService(
        IEnumerable<GenerationDto> generations,
        IEnumerable<ModelDto> models,
        IEnumerable<ManufacturerDto> manufacturers)
    {
        _generations = generations.ToList();
        _modelsById = models.ToDictionary(model => model.Id);
        _manufacturersById = manufacturers.ToDictionary(manufacturer => manufacturer.Id);
    }

    /// <inheritdoc/>
    public IEnumerable<VehicleGeneration> Search(GenerationSearch search)
    {
        ArgumentNullException.ThrowIfNull(search);

        var query = _generations.AsEnumerable();

        if (search.ModelId.HasValue)
        {
            query = query.Where(generation => generation.ModelId == search.ModelId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.Name))
        {
            query = query.Where(generation =>
                generation.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
        }

        return query
            .Select(generation =>
            {
                if (!_modelsById.TryGetValue(generation.ModelId, out var model))
                {
                    throw new InvalidOperationException($"model {generation.ModelId} was not found for generation.");
                }

                if (!_manufacturersById.TryGetValue(model.ManufacturerId, out var manufacturer))
                {
                    throw new InvalidOperationException($"manufacturer {model.ManufacturerId} was not found for generation.");
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
            })
            .OrderBy(generation => generation.Name)
            .ToList();
    }
}

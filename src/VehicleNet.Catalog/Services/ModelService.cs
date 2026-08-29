using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class ModelService : IModelService
{
    private readonly IReadOnlyList<ModelDto> _models;
    private readonly IReadOnlyDictionary<int, ManufacturerDto> _manufacturersById;

    public ModelService(IEnumerable<ModelDto> models, IEnumerable<ManufacturerDto> manufacturers)
    {
        _models = models.ToList();
        _manufacturersById = manufacturers.ToDictionary(manufacturer => manufacturer.Id);
    }

    public IEnumerable<VehicleModel> Search(ModelSearch search)
    {
        ArgumentNullException.ThrowIfNull(search);

        var query = _models.AsEnumerable();

        if (search.ManufacturerId.HasValue)
        {
            query = query.Where(model => model.ManufacturerId == search.ManufacturerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.Name))
        {
            query = query.Where(model =>
                model.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
        }

        return query
            .Select(model =>
            {
                if (!_manufacturersById.TryGetValue(model.ManufacturerId, out var manufacturer))
                {
                    throw new InvalidOperationException($"manufacturer {model.ManufacturerId} was not found for model.");
                }

                return new VehicleModel(
                    model.Id,
                    model.Name,
                    model.ManufacturerId,
                    new VehicleManufacturer(
                        manufacturer.Id,
                        manufacturer.Name,
                        manufacturer.Manufacturer));
            })
            .OrderBy(model => model.Name)
            .ToList();
    }
}

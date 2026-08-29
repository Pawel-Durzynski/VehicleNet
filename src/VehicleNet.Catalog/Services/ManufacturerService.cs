using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class ManufacturerService : IManufacturerService
{
    private readonly IReadOnlyList<ManufacturerDto> _manufacturers;

    public ManufacturerService(IEnumerable<ManufacturerDto> manufacturers)
    {
        _manufacturers = manufacturers.ToList();
    }

    public IEnumerable<VehicleManufacturer> Search(ManufacturerSearch search)
    {
        ArgumentNullException.ThrowIfNull(search);

        var query = _manufacturers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search.Name))
        {
            query = query.Where(manufacturer =>
                manufacturer.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
        }

        return query
            .Select(manufacturer => new VehicleManufacturer(
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.Manufacturer))
            .OrderBy(manufacturer => manufacturer.Name)
            .ToList();
    }
}

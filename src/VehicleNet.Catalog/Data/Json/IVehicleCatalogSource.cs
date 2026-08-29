using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Catalog.Data.Json;

public interface IVehicleCatalogSource
{
    Task<IReadOnlyList<VehicleBodyEngine>> LoadAsync(CancellationToken cancellationToken = default);
}

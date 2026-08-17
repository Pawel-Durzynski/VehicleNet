using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Catalog.Data.Json;

public interface IVehicleCatalogSource
{
    Task<IReadOnlyList<VehicleSpec>> LoadAsync(CancellationToken cancellationToken = default);
}

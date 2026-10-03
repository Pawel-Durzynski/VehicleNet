using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Catalog.Data.Json;

/// <summary>
/// Represents a source of vehicle catalog data that can be loaded asynchronously.
/// </summary>
public interface IVehicleCatalogSource
{
    /// <summary>
    /// Loads the vehicle catalog data from the source.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of vehicle body-engine combinations.</returns>
    Task<IReadOnlyList<VehicleBodyEngine>> LoadAsync(CancellationToken cancellationToken = default);
}

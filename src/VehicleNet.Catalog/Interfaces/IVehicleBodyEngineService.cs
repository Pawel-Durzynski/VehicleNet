using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle body engines based on specified criteria.
/// </summary>
public interface IVehicleBodyEngineService
{
    /// <summary>
    /// Searches for vehicle body engines based on the specified search criteria.
    /// </summary>
    /// <param name="criteria">The search criteria for filtering vehicle body engines.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="VehicleBodyEngineSearchResult"/> object that contains the search results.</returns>
    VehicleBodyEngineSearchResult Search(VehicleBodyEngineSearchCriteria criteria, CancellationToken cancellationToken = default);
}

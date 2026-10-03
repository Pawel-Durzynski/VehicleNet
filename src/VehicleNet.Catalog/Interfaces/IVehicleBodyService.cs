using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle bodies based on specified criteria.
/// </summary>
public interface IVehicleBodyService
{
    /// <summary>
    /// Searches for vehicle bodies based on the provided search criteria.
    /// </summary>
    /// <param name="criteria">The search criteria for filtering vehicle bodies.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="VehicleBodySearchResult"/> object that contains the search results.</returns>
    VehicleBodySearchResult Search(VehicleBodySearchCriteria criteria, CancellationToken cancellationToken = default);
}

using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle body engine variants based on specified criteria.
/// </summary>
public interface IVehicleBodyEngineVariantService
{
    /// <summary>
    /// Searches for vehicle body engine variants based on the provided search criteria.
    /// </summary>
    /// <param name="search">The search criteria for filtering vehicle body engine variants.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="VehicleBodyEngineVariantSearchResult"/> object that contains the search results.</returns>
    VehicleBodyEngineVariantSearchResult Search(VehicleBodyEngineVariantSearch search, CancellationToken cancellationToken = default);
}

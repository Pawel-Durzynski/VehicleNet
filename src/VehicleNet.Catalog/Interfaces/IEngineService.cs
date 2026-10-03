using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle engines based on specified criteria.
/// </summary>
public interface IEngineService
{
    /// <summary>
    /// Searches for vehicle engines based on the provided search criteria.
    /// </summary>
    /// <param name="search">The search criteria for filtering vehicle engines.</param>
    /// <returns>A collection of <see cref="VehicleEngine"/> objects that match the search criteria.</returns>
    IEnumerable<VehicleEngine> Search(EngineSearch search);
}

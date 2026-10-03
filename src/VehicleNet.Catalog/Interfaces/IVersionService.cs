using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle versions based on specified criteria.
/// </summary>
public interface IVersionService
{
    /// <summary>
    /// Searches for vehicle versions based on the provided search criteria.
    /// </summary>
    /// <param name="search">The search criteria for filtering vehicle versions.</param>
    /// <returns>A collection of <see cref="VehicleVersion"/> objects that match the search criteria.</returns>
    IEnumerable<VehicleVersion> Search(VersionSearch search);
}

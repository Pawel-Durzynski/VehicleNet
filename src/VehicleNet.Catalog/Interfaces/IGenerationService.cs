using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle generations based on specified criteria.
/// </summary>
public interface IGenerationService
{
    /// <summary>
    /// Searches for vehicle generations based on the specified search criteria.
    /// </summary>
    /// <param name="search">The search criteria for filtering vehicle generations.</param>
    /// <returns>A collection of <see cref="VehicleGeneration"/> objects that match the search criteria.</returns>
    IEnumerable<VehicleGeneration> Search(GenerationSearch search);
}

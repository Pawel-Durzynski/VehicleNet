using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching engine variants based on specified criteria.
/// </summary>
public interface IEngineVariantService
{
    /// <summary>
    /// Searches for engine variants based on the provided search criteria.
    /// </summary>
    /// <param name="search">The search criteria for filtering engine variants.</param>
    /// <returns>A collection of <see cref="EngineVariant"/> objects that match the search criteria.</returns>
    IEnumerable<EngineVariant> Search(EngineVariantSearch search);
}

using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle models based on specified criteria.
/// </summary>
public interface IModelService
{
    /// <summary>
    /// Searches for vehicle models based on the provided search criteria.
    /// </summary>
    /// <param name="search">The search criteria for filtering vehicle models.</param>
    /// <returns>A collection of <see cref="VehicleModel"/> objects that match the search criteria.</returns>
    IEnumerable<VehicleModel> Search(ModelSearch search);
}

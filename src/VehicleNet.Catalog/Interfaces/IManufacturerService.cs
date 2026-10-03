using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

/// <summary>
/// Defines a service interface for searching vehicle manufacturers based on specified criteria.
/// </summary>
public interface IManufacturerService
{
    /// <summary>
    /// Searches for vehicle manufacturers based on the specified search criteria.
    /// </summary>
    /// <param name="search">The search criteria for filtering vehicle manufacturers.</param>
    /// <returns>A collection of <see cref="VehicleManufacturer"/> objects that match the search criteria.</returns>
    IEnumerable<VehicleManufacturer> Search(ManufacturerSearch search);
}

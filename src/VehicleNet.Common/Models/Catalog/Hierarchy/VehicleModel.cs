namespace VehicleNet.Common.Models.Catalog.Hierarchy;

/// <summary>Represents a vehicle model in the catalog hierarchy.</summary>
/// <param name="Id">The model identifier.</param>
/// <param name="Name">The model name.</param>
/// <param name="ManufacturerId">The parent manufacturer identifier.</param>
/// <param name="Manufacturer">The parent manufacturer.</param>
public sealed record VehicleModel(
    int Id,
    string Name,
    int ManufacturerId,
    VehicleManufacturer Manufacturer);

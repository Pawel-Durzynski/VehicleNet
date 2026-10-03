using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Catalog.Hierarchy;

/// <summary>Represents a vehicle manufacturer in the catalog hierarchy.</summary>
/// <param name="Id">The manufacturer identifier.</param>
/// <param name="Name">The manufacturer name.</param>
/// <param name="Manufacturer">The normalized manufacturer value.</param>
public record VehicleManufacturer(int Id, string Name, Manufacturer Manufacturer);

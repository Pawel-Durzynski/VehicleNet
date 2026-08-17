namespace VehicleNet.Common.Models.Catalog.Hierarchy;

public sealed record VehicleModel(
    int Id,
    string Name,
    int ManufacturerId,
    VehicleManufacturer Manufacturer);

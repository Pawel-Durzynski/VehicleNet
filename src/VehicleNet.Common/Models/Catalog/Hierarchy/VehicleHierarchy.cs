namespace VehicleNet.Common.Models.Catalog.Hierarchy;

public sealed record VehicleHierarchy(
    string Manufacturer,
    string Model,
    string Generation,
    string Version,
    string Engine);

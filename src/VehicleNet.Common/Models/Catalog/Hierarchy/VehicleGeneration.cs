namespace VehicleNet.Common.Models.Catalog.Hierarchy;

public record VehicleGeneration(
    int Id,
    int ModelId,
    string Name,
    int StartYear,
    int? EndYear,
    VehicleModel Model,
    bool ContainsVersions
);

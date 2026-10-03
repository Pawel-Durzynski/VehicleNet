namespace VehicleNet.Common.Models.Catalog.Hierarchy;

/// <summary>Represents a vehicle generation in the catalog hierarchy.</summary>
/// <param name="Id">The generation identifier.</param>
/// <param name="ModelId">The parent model identifier.</param>
/// <param name="Name">The generation name.</param>
/// <param name="StartYear">The first production year.</param>
/// <param name="EndYear">The optional final production year.</param>
/// <param name="Model">The parent vehicle model.</param>
/// <param name="ContainsVersions">Indicates whether the generation contains a version level.</param>
public record VehicleGeneration(
    int Id,
    int ModelId,
    string Name,
    int StartYear,
    int? EndYear,
    VehicleModel Model,
    bool ContainsVersions
);

namespace VehicleNet.Common.Models.Catalog.Hierarchy;

public sealed record VehicleEngine(
    int Id,
    string Name,
    int? GenerationId,
    VehicleGeneration? Generation,
    int? VersionId,
    VehicleVersion? Version);

namespace VehicleNet.Common.Models.Catalog.Hierarchy;

/// <summary>Represents an engine in the vehicle catalog hierarchy.</summary>
/// <param name="Id">The engine identifier.</param>
/// <param name="Name">The engine name.</param>
/// <param name="GenerationId">The optional parent generation identifier.</param>
/// <param name="Generation">The optional parent generation.</param>
/// <param name="VersionId">The optional parent version identifier.</param>
/// <param name="Version">The optional parent version.</param>
public sealed record VehicleEngine(
    int Id,
    string Name,
    int? GenerationId,
    VehicleGeneration? Generation,
    int? VersionId,
    VehicleVersion? Version);

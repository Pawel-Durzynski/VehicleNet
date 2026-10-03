using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Represents a named variant of a vehicle engine.</summary>
public record EngineVariant
{
    /// <summary>Gets the engine variant identifier.</summary>
    public required int EngineVariantId { get; init; }

    /// <summary>Gets the parent engine identifier.</summary>
    public required int EngineId { get; init; }

    /// <summary>Gets the engine variant name.</summary>
    public required string Name { get; init; }

    /// <summary>Gets the parent engine.</summary>
    public required VehicleEngine Engine { get; init; }
}

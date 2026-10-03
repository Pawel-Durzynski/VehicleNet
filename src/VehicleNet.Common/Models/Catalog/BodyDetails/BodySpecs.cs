namespace VehicleNet.Common.Models.Catalog.BodyDetails;

/// <summary>Provides the specifications for a vehicle body.</summary>
public record BodySpecs
{
    /// <summary>Gets the basic body parameters.</summary>
    public BasicParameters? BasicParameters { get; init; }

    /// <summary>Gets the external body dimensions.</summary>
    public ExternalDimensions? ExternalDimensions { get; init; }

    /// <summary>Gets the trunk dimensions.</summary>
    public TrunkDimensions? TrunkDimensions { get; init; }
}

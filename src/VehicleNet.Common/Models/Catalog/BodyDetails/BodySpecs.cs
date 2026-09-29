namespace VehicleNet.Common.Models.Catalog.BodyDetails;

public record BodySpecs
{
    public BasicParameters? BasicParameters { get; init; }

    public ExternalDimensions? ExternalDimensions { get; init; }

    public TrunkDimensions? TrunkDimensions { get; init; }
}

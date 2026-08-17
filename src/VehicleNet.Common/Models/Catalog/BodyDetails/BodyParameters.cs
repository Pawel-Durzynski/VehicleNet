namespace VehicleNet.Common.Models.Catalog.BodyDetails;

public record BodyParameters
{
    public BasicParameters BasicParameters { get; init; } = new();

    public ExternalDimensions ExternalDimensions { get; init; } = new();

    public TrunkDimensions TrunkDimensions { get; init; } = new();
}

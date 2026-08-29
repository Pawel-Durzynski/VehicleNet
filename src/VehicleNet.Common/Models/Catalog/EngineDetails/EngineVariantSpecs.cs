namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EngineVariantSpecs
{
    public required DrivetrainSpecs DrivetrainSpecs { get; init; }

    public required PerformanceSpecs PerformanceSpecs { get; init; }
}

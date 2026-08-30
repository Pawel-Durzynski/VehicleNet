namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EngineVariantSpecs
{
    public DrivetrainSpecs? DrivetrainSpecs { get; init; }

    public PerformanceSpecs? PerformanceSpecs { get; init; }
}

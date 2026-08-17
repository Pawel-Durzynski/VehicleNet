namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record TechnicalSpecs
{
    public EngineSpecs EngineSpecs { get; init; } = new();

    public DrivetrainSpecs DrivetrainSpecs { get; init; } = new();

    public PerformanceSpecs PerformanceSpecs { get; init; } = new();
}

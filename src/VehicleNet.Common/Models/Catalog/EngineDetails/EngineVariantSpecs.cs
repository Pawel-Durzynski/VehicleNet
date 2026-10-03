namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Provides specifications specific to an engine variant.</summary>
public record EngineVariantSpecs
{
    /// <summary>Gets the drivetrain specifications.</summary>
    public DrivetrainSpecs? DrivetrainSpecs { get; init; }

    /// <summary>Gets the performance specifications.</summary>
    public PerformanceSpecs? PerformanceSpecs { get; init; }
}

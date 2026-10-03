using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Provides vehicle performance specifications.</summary>
public record PerformanceSpecs
{
    /// <summary>Gets the acceleration time from zero to 100 kilometers per hour.</summary>
    public ParameterValue Acceleration0To100 { get; init; } = ParameterValue.Missing(MeasurementUnit.Second);

    /// <summary>Gets the maximum vehicle speed.</summary>
    public ParameterValue TopSpeed { get; init; } = ParameterValue.Missing(MeasurementUnit.KilometerPerHour);
}

using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record PerformanceSpecs
{
    public ParameterValue Acceleration0To100 { get; init; } = ParameterValue.Missing(MeasurementUnit.Second);

    public ParameterValue TopSpeed { get; init; } = ParameterValue.Missing(MeasurementUnit.KilometerPerHour);
}

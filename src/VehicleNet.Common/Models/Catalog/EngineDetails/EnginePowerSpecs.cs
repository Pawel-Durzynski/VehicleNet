using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EnginePowerSpecs
{
    public ParameterValue Horsepower { get; init; } = ParameterValue.Missing(MeasurementUnit.Horsepower);

    public ParameterValue AtRpm { get; init; } = ParameterValue.Missing(MeasurementUnit.Rpm);
}

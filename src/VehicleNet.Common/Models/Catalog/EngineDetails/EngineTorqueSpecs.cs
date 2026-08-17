using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EngineTorqueSpecs
{
    public ParameterValue MaxTorque { get; init; } = ParameterValue.Missing(MeasurementUnit.NewtonMeter);

    public ParameterValue AtRpmFrom { get; init; } = ParameterValue.Missing(MeasurementUnit.Rpm);

    public ParameterValue AtRpmTo { get; init; } = ParameterValue.Missing(MeasurementUnit.Rpm);
}

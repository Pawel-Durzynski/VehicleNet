using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Provides engine torque specifications.</summary>
public record EngineTorqueSpecs
{
    /// <summary>Gets the maximum engine torque.</summary>
    public ParameterValue MaxTorque { get; init; } = ParameterValue.Missing(MeasurementUnit.NewtonMeter);

    /// <summary>Gets the lower engine-speed bound at which maximum torque is available.</summary>
    public ParameterValue AtRpmFrom { get; init; } = ParameterValue.Missing(MeasurementUnit.Rpm);

    /// <summary>Gets the upper engine-speed bound at which maximum torque is available.</summary>
    public ParameterValue AtRpmTo { get; init; } = ParameterValue.Missing(MeasurementUnit.Rpm);
}

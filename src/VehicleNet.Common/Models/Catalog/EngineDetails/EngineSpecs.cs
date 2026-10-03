using VehicleNet.Common.Enums;
using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Provides technical specifications for an engine.</summary>
public record EngineSpecs
{
    /// <summary>Gets the engine displacement.</summary>
    public ParameterValue Capacity { get; init; } = ParameterValue.Missing(MeasurementUnit.CubicCentimeter);

    /// <summary>Gets the engine fuel type.</summary>
    public FuelType FuelType { get; init; } = FuelType.Unknown;

    /// <summary>Gets the engine architecture.</summary>
    public EngineArchitecture? Architecture { get; init; }

    /// <summary>Gets the engine power specifications.</summary>
    public EnginePowerSpecs? Power { get; init; }

    /// <summary>Gets the engine torque specifications.</summary>
    public EngineTorqueSpecs? Torque { get; init; }
}

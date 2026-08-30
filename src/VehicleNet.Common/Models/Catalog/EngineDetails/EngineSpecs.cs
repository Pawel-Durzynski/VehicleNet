using VehicleNet.Common.Enums;
using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EngineSpecs
{
    public ParameterValue Capacity { get; init; } = ParameterValue.Missing(MeasurementUnit.CubicCentimeter);

    public FuelType FuelType { get; init; } = FuelType.Unknown;

    public EngineArchitecture? Architecture { get; init; }

    public EnginePowerSpecs? Power { get; init; }

    public EngineTorqueSpecs? Torque { get; init; }
}

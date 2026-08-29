using VehicleNet.Common.Enums;
using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EngineSpecs
{
    public ParameterValue Capacity { get; init; } = ParameterValue.Missing(MeasurementUnit.CubicCentimeter);

    public FuelType FuelType { get; init; } = FuelType.Unknown;

    public required EngineArchitecture Architecture { get; init; }

    public required EnginePowerSpecs Power { get; init; }

    public required EngineTorqueSpecs Torque { get; init; }
}

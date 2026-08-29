using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.BodyDetails;

public sealed record TrunkDimensions
{
    public ParameterValue MaximumTrunkCapacitySeatsFolded { get; init; } = ParameterValue.Missing(MeasurementUnit.Liter); // Maksymalna pojemność bagażnika (siedzenia złożone)

    public ParameterValue MinimumTrunkCapacitySeatsUp { get; init; } = ParameterValue.Missing(MeasurementUnit.Liter); // Minimalna pojemność bagażnika (siedzenia rozłożone)
}

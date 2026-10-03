using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.BodyDetails;

/// <summary>Provides vehicle trunk capacity measurements.</summary>
public sealed record TrunkDimensions
{
    /// <summary>Gets the maximum trunk capacity with the seats folded.</summary>
    public ParameterValue MaximumTrunkCapacitySeatsFolded { get; init; } = ParameterValue.Missing(MeasurementUnit.Liter); // Maksymalna pojemność bagażnika (siedzenia złożone)

    /// <summary>Gets the minimum trunk capacity with the seats raised.</summary>
    public ParameterValue MinimumTrunkCapacitySeatsUp { get; init; } = ParameterValue.Missing(MeasurementUnit.Liter); // Minimalna pojemność bagażnika (siedzenia rozłożone)
}

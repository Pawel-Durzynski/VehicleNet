using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.BodyDetails;

/// <summary>Provides basic vehicle body parameters.</summary>
public sealed record BasicParameters
{
    /// <summary>Gets the number of doors.</summary>
    public ParameterValue NumberOfDoors { get; init; } = ParameterValue.Missing(MeasurementUnit.Count); // Liczba drzwi

    /// <summary>Gets the number of seats.</summary>
    public ParameterValue NumberOfSeats { get; init; } = ParameterValue.Missing(MeasurementUnit.Count); // Liczba miejsc

    /// <summary>Gets the turning diameter.</summary>
    public ParameterValue TurningDiameter { get; init; } = ParameterValue.Missing(MeasurementUnit.Meter); // Średnica zawracania

    /// <summary>Gets the turning radius.</summary>
    public ParameterValue TurningRadius { get; init; } = ParameterValue.Missing(MeasurementUnit.Meter); // Promień skrętu
}

using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.BodyDetails;

public sealed record BasicParameters
{
    public ParameterValue NumberOfDoors { get; init; } = ParameterValue.Missing(MeasurementUnit.Count); // Liczba drzwi

    public ParameterValue NumberOfSeats { get; init; } = ParameterValue.Missing(MeasurementUnit.Count); // Liczba miejsc

    public ParameterValue TurningDiameter { get; init; } = ParameterValue.Missing(MeasurementUnit.Meter); // Średnica zawracania

    public ParameterValue TurningRadius { get; init; } = ParameterValue.Missing(MeasurementUnit.Meter); // Promień skrętu
}

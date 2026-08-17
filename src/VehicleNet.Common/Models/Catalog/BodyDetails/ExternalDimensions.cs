using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.BodyDetails;

public sealed record ExternalDimensions
{
    public ParameterValue Length { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Długość

    public ParameterValue Width { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Szerokość

    public ParameterValue Height { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Wysokość

    public ParameterValue Wheelbase { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Rozstaw osi

    public ParameterValue GroundClearance { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Prześwit
}

using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.BodyDetails;

/// <summary>Provides the external dimensions of a vehicle body.</summary>
public sealed record ExternalDimensions
{
    /// <summary>Gets the vehicle length.</summary>
    public ParameterValue Length { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Długość

    /// <summary>Gets the vehicle width.</summary>
    public ParameterValue Width { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Szerokość

    /// <summary>Gets the vehicle height.</summary>
    public ParameterValue Height { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Wysokość

    /// <summary>Gets the wheelbase.</summary>
    public ParameterValue Wheelbase { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Rozstaw osi

    /// <summary>Gets the ground clearance.</summary>
    public ParameterValue GroundClearance { get; init; } = ParameterValue.Missing(MeasurementUnit.Millimeter); // Prześwit
}

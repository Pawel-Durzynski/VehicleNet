namespace VehicleNet.Common.Models.Units;

/// <summary>Represents an optional measured vehicle specification value.</summary>
public sealed record ParameterValue
{
    /// <summary>Initializes a parameter value.</summary>
    /// <param name="value">The numeric value.</param>
    /// <param name="unit">The measurement unit.</param>
    /// <param name="isMissing">Indicates whether the source value is missing.</param>
    public ParameterValue(decimal? value, MeasurementUnit? unit, bool isMissing = false)
    {
        Value = value;
        IsMissing = isMissing;
        Unit = isMissing ? null : unit;
    }

    /// <summary>Gets the numeric value.</summary>
    public decimal? Value { get; init; }

    /// <summary>Gets the measurement unit.</summary>
    public MeasurementUnit? Unit { get; init; }

    /// <summary>Gets a value indicating whether the source value is missing.</summary>
    public bool IsMissing { get; init; }

    /// <summary>Creates a missing parameter value.</summary>
    public static ParameterValue Missing(MeasurementUnit unit = MeasurementUnit.None) =>
        new(null, null, true);

    /// <summary>Creates a parameter value with a numeric value and unit.</summary>
    public static ParameterValue Create(decimal value, MeasurementUnit unit = MeasurementUnit.None) =>
        new(value, unit, false);

    /// <summary>Gets a value indicating whether a non-missing numeric value is available.</summary>
    public bool HasValue => Value.HasValue && !IsMissing;
}

namespace VehicleNet.Common.Models.Units;

public sealed record ParameterValue
{
    public ParameterValue(decimal? value, MeasurementUnit? unit, bool isMissing = false)
    {
        Value = value;
        IsMissing = isMissing;
        Unit = isMissing ? null : unit;
    }

    public decimal? Value { get; init; }

    public MeasurementUnit? Unit { get; init; }

    public bool IsMissing { get; init; }

    public static ParameterValue Missing(MeasurementUnit unit = MeasurementUnit.None) =>
        new(null, null, true);

    public static ParameterValue Create(decimal value, MeasurementUnit unit = MeasurementUnit.None) =>
        new(value, unit, false);

    public bool HasValue => Value.HasValue && !IsMissing;
}

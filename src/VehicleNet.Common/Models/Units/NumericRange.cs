namespace VehicleNet.Common.Models.Units;

/// <summary>Represents an inclusive numeric range.</summary>
/// <param name="Min">The optional minimum value.</param>
/// <param name="Max">The optional maximum value.</param>
public sealed record NumericRange(decimal? Min = null, decimal? Max = null)
{
    /// <summary>Determines whether a value is within the range.</summary>
    /// <param name="value">The value to test.</param>
    /// <returns><see langword="true"/> when the value is within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(decimal value)
    {
        if (Min.HasValue && value < Min.Value)
        {
            return false;
        }

        if (Max.HasValue && value > Max.Value)
        {
            return false;
        }

        return true;
    }
}

namespace VehicleNet.Common.Models.Units;

public sealed record NumericRange(decimal? Min = null, decimal? Max = null)
{
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

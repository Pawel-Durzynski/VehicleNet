namespace VehicleNet.Common.Enums;

/// <summary>
/// Identifies the fuel or energy source used by a vehicle.
/// </summary>
public enum FuelType : byte
{
    /// <summary>The fuel type is unknown.</summary>
    Unknown = 0,
    /// <summary>Petrol.</summary>
    Petrol = 1,
    /// <summary>Diesel fuel.</summary>
    Diesel = 2,
    /// <summary>A hybrid powertrain.</summary>
    Hybrid = 3,
    /// <summary>An electric powertrain.</summary>
    Electric = 4,
    /// <summary>A plug-in hybrid powertrain.</summary>
    PlugInHybrid = 5,
    /// <summary>Liquefied petroleum gas.</summary>
    Lpg = 6,
    /// <summary>Compressed natural gas.</summary>
    Cng = 7,
}

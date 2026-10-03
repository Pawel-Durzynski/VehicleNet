namespace VehicleNet.Common.Enums;

/// <summary>
/// Identifies the wheels driven by a vehicle powertrain.
/// </summary>
public enum Drivetrain : byte
{
    /// <summary>The drivetrain is unknown.</summary>
    Unknown = 0,

    /// <summary>Front-wheel drive.</summary>
    FWD = 1,

    /// <summary>Rear-wheel drive.</summary>
    RWD = 2,

    /// <summary>All-wheel drive.</summary>
    AWD = 3
}

namespace VehicleNet.Common.Enums;

/// <summary>
/// Identifies a vehicle transmission type.
/// </summary>
public enum TransmissionType : byte
{
    /// <summary>The transmission type is unknown.</summary>
    Unknown = 0,
    /// <summary>A manual transmission.</summary>
    Manual = 1,
    /// <summary>An automatic transmission.</summary>
    Automatic = 2,
    /// <summary>An automated manual transmission.</summary>
    AutomatedManual = 3,
    /// <summary>A continuously variable transmission.</summary>
    Cvt = 4,
    /// <summary>A dual-clutch transmission.</summary>
    DualClutch = 5,
}

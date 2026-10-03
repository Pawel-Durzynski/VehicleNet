namespace VehicleNet.Common.Enums;

/// <summary>
/// Identifies a vehicle body style.
/// </summary>
public enum BodyType : byte
{
    /// <summary>The body style is unknown.</summary>
    Unknown = 0,
    /// <summary>A sedan body.</summary>
    Sedan = 1,
    /// <summary>A hatchback body.</summary>
    Hatchback = 2,
    /// <summary>A coupe body.</summary>
    Coupe = 3,
    /// <summary>A convertible body.</summary>
    Convertible = 4,
    /// <summary>A station wagon body.</summary>
    Wagon = 5,
    /// <summary>A sport utility vehicle body.</summary>
    Suv = 6,
    /// <summary>A crossover utility vehicle body.</summary>
    Crossover = 7,
    /// <summary>A pickup truck body.</summary>
    Pickup = 8,
    /// <summary>A minivan body.</summary>
    Minivan = 9,
    /// <summary>A van body.</summary>
    Van = 10,
    /// <summary>A roadster body.</summary>
    Roadster = 11,
    /// <summary>A liftback body.</summary>
    Liftback = 12,
    /// <summary>A fastback body.</summary>
    Fastback = 13,
    /// <summary>A targa-top body.</summary>
    Targa = 14,
}

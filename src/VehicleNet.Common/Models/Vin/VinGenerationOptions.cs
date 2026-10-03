namespace VehicleNet.Common.Models.Vin;

/// <summary>Defines optional constraints for mock VIN generation.</summary>
public sealed record VinGenerationOptions
{
    /// <summary>Gets the optional three-character world manufacturer identifier.</summary>
    public string? WorldManufacturerIdentifier { get; init; }

    /// <summary>Gets the optional model year.</summary>
    public int? ModelYear { get; init; }

    /// <summary>Gets the optional plant code.</summary>
    public char? PlantCode { get; init; }
}

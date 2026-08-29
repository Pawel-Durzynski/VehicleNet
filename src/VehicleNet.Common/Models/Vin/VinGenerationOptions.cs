namespace VehicleNet.Common.Models.Vin;

public sealed record VinGenerationOptions
{
    public string? WorldManufacturerIdentifier { get; init; }

    public int? ModelYear { get; init; }

    public char? PlantCode { get; init; }
}

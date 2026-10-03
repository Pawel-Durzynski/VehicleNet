namespace VehicleNet.Common.Models.Vin;

/// <summary>Contains the parsed components of a vehicle identification number.</summary>
/// <param name="Vin">The complete VIN.</param>
/// <param name="WorldManufacturerIdentifier">The world manufacturer identifier.</param>
/// <param name="VehicleDescriptorSection">The vehicle descriptor section.</param>
/// <param name="VehicleIdentifierSection">The vehicle identifier section.</param>
/// <param name="CheckDigit">The check digit.</param>
/// <param name="ModelYearCode">The model-year code.</param>
/// <param name="ModelYear">The decoded model year, when available.</param>
/// <param name="PlantCode">The manufacturing plant code.</param>
/// <param name="SequentialNumber">The sequential production number.</param>
public sealed record VinParts(
    string Vin,
    string WorldManufacturerIdentifier,
    string VehicleDescriptorSection,
    string VehicleIdentifierSection,
    char CheckDigit,
    char ModelYearCode,
    int? ModelYear,
    char PlantCode,
    string SequentialNumber);

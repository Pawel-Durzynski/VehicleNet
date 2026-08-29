namespace VehicleNet.Common.Models.Vin;

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

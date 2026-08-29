using VehicleNet.Common.Models.Vin;

namespace VehicleNet.Vin.Interfaces;

public interface IVinValidator
{
    VinValidationResult Validate(string? vin);

    bool IsValid(string? vin);
}

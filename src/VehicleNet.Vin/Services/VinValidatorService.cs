using VehicleNet.Common.Consts;
using VehicleNet.Common.Models.Vin;
using VehicleNet.Vin.Interfaces;

namespace VehicleNet.Vin.Services;

public sealed class VinValidatorService : IVinValidator
{

    public VinValidationResult Validate(string? vin)
    {
        if (string.IsNullOrWhiteSpace(vin))
        {
            return VinValidationResult.Failure("VIN cannot be null or empty.");
        }

        vin = vin.Trim().ToUpperInvariant();

        if (vin.Length != 17)
        {
            return VinValidationResult.Failure("VIN must be exactly 17 characters.");
        }

        for (var i = 0; i < vin.Length; i++)
        {
            if (!VinConsts.Transliteration.ContainsKey(vin[i]) || vin[i] is 'I' or 'O' or 'Q')
            {
                return VinValidationResult.Failure($"VIN contains invalid character '{vin[i]}' at position {i + 1}.");
            }
        }

        var expected = CalculateCheckDigit(vin);
        if (vin[8] != expected)
        {
            return VinValidationResult.Failure($"Invalid check digit. Expected '{expected}', found '{vin[8]}'.");
        }

        return VinValidationResult.Success();
    }

    public bool IsValid(string? vin) => Validate(vin).IsValid;

    private static char CalculateCheckDigit(string vin)
    {
        var sum = 0;

        for (var i = 0; i < vin.Length; i++)
        {
            var character = char.ToUpperInvariant(vin[i]);
            var value = VinConsts.Transliteration[character];
            sum += value * VinConsts.Weights[i];
        }

        var remainder = sum % 11;
        return remainder == 10 ? 'X' : (char)('0' + remainder);
    }
}

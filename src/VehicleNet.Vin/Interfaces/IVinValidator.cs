using VehicleNet.Common.Models.Vin;

namespace VehicleNet.Vin.Interfaces;

/// <summary>
/// Defines an interface for validating Vehicle Identification Numbers (VINs).
/// </summary>
public interface IVinValidator
{
    /// <summary>
    /// Validates the provided VIN and returns a VinValidationResult indicating whether the VIN is valid or not.
    /// </summary>
    /// <param name="vin">The Vehicle Identification Number (VIN) to validate.</param>
    /// <returns>A VinValidationResult indicating whether the VIN is valid or not.</returns>
    VinValidationResult Validate(string? vin);

    /// <summary>
    /// Determines whether the provided VIN is valid or not.    
    /// </summary>
    /// <param name="vin">The Vehicle Identification Number (VIN) to check.</param>
    /// <returns>True if the VIN is valid; otherwise, false.</returns>
    bool IsValid(string? vin);
}

using VehicleNet.Common.Models.Vin;

namespace VehicleNet.Vin.Interfaces;

/// <summary>
/// Defines an interface for generating mock Vehicle Identification Numbers (VINs).
/// </summary>
public interface IVinGenerator
{
    /// <summary>
    /// Generates a mock Vehicle Identification Number (VIN) based on the provided options and random number generator.
    /// </summary>
    /// <param name="options">The options to use for VIN generation.</param>
    /// <param name="random">The random number generator to use.</param>
    /// <returns>A mock Vehicle Identification Number (VIN).</returns>
    string GenerateMockVin(VinGenerationOptions? options = null, Random? random = null);
}

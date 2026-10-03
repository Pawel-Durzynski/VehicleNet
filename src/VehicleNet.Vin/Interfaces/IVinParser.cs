using VehicleNet.Common.Models.Vin;

namespace VehicleNet.Vin.Interfaces;

/// <summary>
/// Defines an interface for parsing Vehicle Identification Numbers (VINs) into their constituent parts.
/// </summary>
public interface IVinParser
{
    /// <summary>
    /// Parses a given VIN string and returns its constituent parts encapsulated in a VinParts object.
    /// </summary>
    /// <param name="vin">The Vehicle Identification Number (VIN) to parse.</param>
    /// <returns>A VinParts object containing the constituent parts of the VIN.</returns>
    VinParts Parse(string vin);
}

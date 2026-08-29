using VehicleNet.Common.Models.Vin;

namespace VehicleNet.Vin.Interfaces;

public interface IVinGenerator
{
    string GenerateMockVin(VinGenerationOptions? options = null, Random? random = null);
}

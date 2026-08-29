using VehicleNet.Common.Models.Vin;

namespace VehicleNet.Vin.Interfaces;

public interface IVinParser
{
    VinParts Parse(string vin);
}

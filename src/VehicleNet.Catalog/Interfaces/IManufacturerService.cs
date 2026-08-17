using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IManufacturerService
{
    IEnumerable<VehicleManufacturer> Search(ManufacturerSearch search);
}

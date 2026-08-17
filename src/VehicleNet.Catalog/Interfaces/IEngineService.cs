using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IEngineService
{
    IEnumerable<VehicleEngine> Search(EngineSearch search);
}

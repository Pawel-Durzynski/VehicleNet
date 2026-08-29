using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IVehicleBodyEngineService
{
    VehicleBodyEngineSearchResult Search(VehicleBodyEngineSearchCriteria criteria, CancellationToken cancellationToken = default);
}

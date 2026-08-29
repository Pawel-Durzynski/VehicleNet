using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IVehicleBodyService
{
    VehicleBodySearchResult Search(VehicleBodySearchCriteria criteria, CancellationToken cancellationToken = default);
}

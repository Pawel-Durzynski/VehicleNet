using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IVehicleSpecService
{
    VehicleSpecSearchResult Search(VehicleSpecSearchCriteria criteria, CancellationToken cancellationToken = default);
}

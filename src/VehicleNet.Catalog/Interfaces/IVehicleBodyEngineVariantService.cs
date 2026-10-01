using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IVehicleBodyEngineVariantService
{
    VehicleBodyEngineVariantSearchResult Search(VehicleBodyEngineVariantSearch search, CancellationToken cancellationToken = default);
}

using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IEngineVariantService
{
    IEnumerable<EngineVariant> Search(EngineVariantSearch search);
}

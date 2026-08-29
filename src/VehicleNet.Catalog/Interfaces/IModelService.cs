using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IModelService
{
    IEnumerable<VehicleModel> Search(ModelSearch search);
}

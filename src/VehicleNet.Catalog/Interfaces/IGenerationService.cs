using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IGenerationService
{
    IEnumerable<VehicleGeneration> Search(GenerationSearch search);
}

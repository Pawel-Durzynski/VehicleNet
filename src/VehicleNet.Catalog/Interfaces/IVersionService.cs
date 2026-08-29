using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Interfaces;

public interface IVersionService
{
    IEnumerable<VehicleVersion> Search(VersionSearch search);
}

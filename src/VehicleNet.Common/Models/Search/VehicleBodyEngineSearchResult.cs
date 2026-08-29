using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Common.Models.Search;

public sealed record VehicleBodyEngineSearchResult(
    int TotalCount,
    IReadOnlyList<VehicleBodyEngine> Items);

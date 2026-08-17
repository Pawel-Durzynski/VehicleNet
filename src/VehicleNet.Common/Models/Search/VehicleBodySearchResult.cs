using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Common.Models.Search;

public sealed record VehicleBodySearchResult(
    int TotalCount,
    IReadOnlyList<VehicleBody> Items);

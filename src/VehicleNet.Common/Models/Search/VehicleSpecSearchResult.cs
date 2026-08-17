using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Common.Models.Search;

public sealed record VehicleSpecSearchResult(
    int TotalCount,
    IReadOnlyList<VehicleSpec> Items);

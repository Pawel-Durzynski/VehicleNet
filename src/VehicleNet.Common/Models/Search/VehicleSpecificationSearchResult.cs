using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Common.Models.Search;

public sealed record VehicleBodyEngineVariantSearchResult(
    int TotalCount,
    IReadOnlyList<VehicleBodyEngineVariant> Items);

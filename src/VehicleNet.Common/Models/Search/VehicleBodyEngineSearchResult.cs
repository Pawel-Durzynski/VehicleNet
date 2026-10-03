using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Common.Models.Search;

/// <summary>Contains vehicle body-engine search results.</summary>
/// <param name="TotalCount">The total number of matching items.</param>
/// <param name="Items">The matching items.</param>
public sealed record VehicleBodyEngineSearchResult(
    int TotalCount,
    IReadOnlyList<VehicleBodyEngine> Items);

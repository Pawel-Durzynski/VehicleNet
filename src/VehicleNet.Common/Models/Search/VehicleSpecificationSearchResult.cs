using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Common.Models.Search;

/// <summary>Contains body-engine-variant search results.</summary>
/// <param name="TotalCount">The total number of matching items.</param>
/// <param name="Items">The matching items.</param>
public sealed record VehicleBodyEngineVariantSearchResult(
    int TotalCount,
    IReadOnlyList<VehicleBodyEngineVariant> Items);

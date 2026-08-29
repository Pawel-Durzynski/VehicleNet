namespace VehicleNet.Common.Models.Search;

public sealed record EngineVariantSearch
{
    public int? EngineId { get; init; }

    public string? Name { get; init; }
}

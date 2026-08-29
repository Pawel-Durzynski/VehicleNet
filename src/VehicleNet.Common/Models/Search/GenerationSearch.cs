namespace VehicleNet.Common.Models.Search;

public sealed record GenerationSearch
{
    public int? ModelId { get; init; }

    public string? Name { get; init; }
}

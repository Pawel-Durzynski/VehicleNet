namespace VehicleNet.Common.Models.Search;

public sealed record VersionSearch
{
    public int? GenerationId { get; init; }

    public string? Name { get; init; }
}

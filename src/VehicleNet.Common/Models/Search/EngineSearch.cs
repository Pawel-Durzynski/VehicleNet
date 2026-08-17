namespace VehicleNet.Common.Models.Search;

public sealed record EngineSearch
{
    public int? GenerationId { get; init; }

    public int? VersionId { get; init; }

    public string? Name { get; init; }
}

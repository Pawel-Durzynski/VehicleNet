namespace VehicleNet.Common.Models.Search;

public sealed record VehicleBodySearchCriteria
{
    public int? VehicleBodyId { get; init; }

    public string? Manufacturer { get; init; }

    public string? Model { get; init; }

    public int? GenerationId { get; init; }

    public string? Generation { get; init; }

    public int? VersionId { get; init; }

    public string? Version { get; init; }
}

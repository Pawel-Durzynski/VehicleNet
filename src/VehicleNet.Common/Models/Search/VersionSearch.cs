namespace VehicleNet.Common.Models.Search;

/// <summary>Defines filters for searching vehicle versions.</summary>
public sealed record VersionSearch
{
    /// <summary>Gets the generation identifier filter.</summary>
    public int? GenerationId { get; init; }

    /// <summary>Gets the version name filter.</summary>
    public string? Name { get; init; }
}

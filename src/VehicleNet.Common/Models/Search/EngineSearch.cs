namespace VehicleNet.Common.Models.Search;

/// <summary>Defines filters for searching engines.</summary>
public sealed record EngineSearch
{
    /// <summary>Gets the generation identifier filter.</summary>
    public int? GenerationId { get; init; }

    /// <summary>Gets the version identifier filter.</summary>
    public int? VersionId { get; init; }

    /// <summary>Gets the engine name filter.</summary>
    public string? Name { get; init; }
}

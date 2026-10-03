namespace VehicleNet.Common.Models.Search;

/// <summary>Defines filters for searching engine variants.</summary>
public sealed record EngineVariantSearch
{
    /// <summary>Gets the engine identifier filter.</summary>
    public int? EngineId { get; init; }

    /// <summary>Gets the engine variant name filter.</summary>
    public string? Name { get; init; }
}

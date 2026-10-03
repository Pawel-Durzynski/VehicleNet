namespace VehicleNet.Common.Models.Search;

/// <summary>Defines filters for searching vehicle generations.</summary>
public sealed record GenerationSearch
{
    /// <summary>Gets the model identifier filter.</summary>
    public int? ModelId { get; init; }

    /// <summary>Gets the generation name filter.</summary>
    public string? Name { get; init; }
}

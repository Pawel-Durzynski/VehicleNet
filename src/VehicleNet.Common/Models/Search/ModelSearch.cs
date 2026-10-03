namespace VehicleNet.Common.Models.Search;

/// <summary>Defines filters for searching vehicle models.</summary>
public sealed record ModelSearch
{
    /// <summary>Gets the manufacturer identifier filter.</summary>
    public int? ManufacturerId { get; init; }

    /// <summary>Gets the model name filter.</summary>
    public string? Name { get; init; }
}

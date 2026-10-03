namespace VehicleNet.Common.Models.Search;

/// <summary>Defines filters for searching manufacturers.</summary>
public sealed record ManufacturerSearch
{
    /// <summary>Gets the manufacturer name filter.</summary>
    public string? Name { get; init; }
}

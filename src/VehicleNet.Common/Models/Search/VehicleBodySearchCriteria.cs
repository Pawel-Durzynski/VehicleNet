namespace VehicleNet.Common.Models.Search;

/// <summary>Defines criteria for searching vehicle bodies.</summary>
public sealed record VehicleBodySearchCriteria
{
    /// <summary>Gets the vehicle body identifier filter.</summary>
    public int? VehicleBodyId { get; init; }

    /// <summary>Gets the manufacturer name filter.</summary>
    public string? Manufacturer { get; init; }

    /// <summary>Gets the model name filter.</summary>
    public string? Model { get; init; }

    /// <summary>Gets the generation identifier filter.</summary>
    public int? GenerationId { get; init; }

    /// <summary>Gets the generation name filter.</summary>
    public string? Generation { get; init; }

    /// <summary>Gets the version identifier filter.</summary>
    public int? VersionId { get; init; }

    /// <summary>Gets the version name filter.</summary>
    public string? Version { get; init; }
}

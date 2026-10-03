using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Search;

/// <summary>Defines filters for searching body-engine variants.</summary>
public sealed record VehicleBodyEngineVariantSearch
{
    /// <summary>Gets or sets the engine variant identifier filter.</summary>
    public int? EngineVariantId { get; set; }

    /// <summary>Gets the body-engine identifier filter.</summary>
    public int? VehicleBodyEngineId { get; init; }

    /// <summary>Gets the transmission type filter.</summary>
    public TransmissionType? TransmissionType { get; init; }

    /// <summary>Gets the drivetrain filter.</summary>
    public Drivetrain? Drivetrain { get; init; }
}

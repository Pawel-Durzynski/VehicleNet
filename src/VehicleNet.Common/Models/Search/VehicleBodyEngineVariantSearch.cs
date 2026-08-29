using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Search;

public sealed record VehicleBodyEngineVariantSearch
{
    public int? EngineVariantId { get; set; }

    public int? VehicleBodyEngineId { get; init; }

    public TransmissionType? TransmissionType { get; init; }

    public Drivetrain? Drivetrain { get; init; }
}

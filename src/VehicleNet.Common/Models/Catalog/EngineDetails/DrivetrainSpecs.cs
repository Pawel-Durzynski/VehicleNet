using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record DrivetrainSpecs
{
    public TransmissionType TransmissionType { get; init; } = TransmissionType.Unknown;

    public Drivetrain Drivetrain { get; init; } = Drivetrain.Unknown;
}

using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Provides transmission and drivetrain specifications.</summary>
public record DrivetrainSpecs
{
    /// <summary>Gets the transmission type.</summary>
    public TransmissionType TransmissionType { get; init; } = TransmissionType.Unknown;

    /// <summary>Gets the drivetrain layout.</summary>
    public Drivetrain Drivetrain { get; init; } = Drivetrain.Unknown;
}

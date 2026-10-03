using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Search;

/// <summary>Builds body-engine-variant search criteria.</summary>
public sealed class VehicleBodyEngineVariantSearchCriteriaBuilder
{
    private int? _engineVariantId;
    private int? _vehicleBodyEngineId;
    private TransmissionType? _transmissionType;
    private Drivetrain? _drivetrain;

    /// <summary>Sets the engine variant identifier filter.</summary>
    public VehicleBodyEngineVariantSearchCriteriaBuilder WithEngineVariantId(int? engineVariantId)
    {
        _engineVariantId = engineVariantId;
        return this;
    }

    /// <summary>Sets the body-engine identifier filter.</summary>
    public VehicleBodyEngineVariantSearchCriteriaBuilder WithVehicleBodyEngineId(int vehicleBodyEngineId)
    {
        _vehicleBodyEngineId = vehicleBodyEngineId;
        return this;
    }

    /// <summary>Sets the transmission type filter.</summary>
    public VehicleBodyEngineVariantSearchCriteriaBuilder WithTransmissionType(TransmissionType transmissionType)
    {
        _transmissionType = transmissionType;
        return this;
    }

    /// <summary>Sets the drivetrain filter.</summary>
    public VehicleBodyEngineVariantSearchCriteriaBuilder WithDrivetrain(Drivetrain drivetrain)
    {
        _drivetrain = drivetrain;
        return this;
    }

    /// <summary>Builds the configured search criteria.</summary>
    public VehicleBodyEngineVariantSearch Build() =>
        new()
        {
            EngineVariantId = _engineVariantId,
            VehicleBodyEngineId = _vehicleBodyEngineId,
            TransmissionType = _transmissionType,
            Drivetrain = _drivetrain
        };
}

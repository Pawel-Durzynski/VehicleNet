using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Search;

public sealed class VehicleBodyEngineVariantSearchCriteriaBuilder
{
    private int? _engineVariantId;
    private int? _vehicleBodyEngineId;
    private TransmissionType? _transmissionType;
    private Drivetrain? _drivetrain;

    public VehicleBodyEngineVariantSearchCriteriaBuilder WithEngineVariantId(int? engineVariantId)
    {
        _engineVariantId = engineVariantId;
        return this;
    }

    public VehicleBodyEngineVariantSearchCriteriaBuilder WithVehicleBodyEngineId(int vehicleBodyEngineId)
    {
        _vehicleBodyEngineId = vehicleBodyEngineId;
        return this;
    }

    public VehicleBodyEngineVariantSearchCriteriaBuilder WithTransmissionType(TransmissionType transmissionType)
    {
        _transmissionType = transmissionType;
        return this;
    }

    public VehicleBodyEngineVariantSearchCriteriaBuilder WithDrivetrain(Drivetrain drivetrain)
    {
        _drivetrain = drivetrain;
        return this;
    }

    public VehicleBodyEngineVariantSearch Build() =>
        new()
        {
            EngineVariantId = _engineVariantId,
            VehicleBodyEngineId = _vehicleBodyEngineId,
            TransmissionType = _transmissionType,
            Drivetrain = _drivetrain
        };
}

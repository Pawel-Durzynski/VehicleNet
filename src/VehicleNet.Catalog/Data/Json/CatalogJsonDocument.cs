using VehicleNet.Common.Enums;
using VehicleNet.Common.Models.Units;

namespace VehicleNet.Catalog.Data.Json;

internal sealed record CatalogJsonDocument
{
    public IReadOnlyList<ManufacturerDto> Manufacturers { get; init; } = [];

    public IReadOnlyList<ModelDto> Models { get; init; } = [];

    public IReadOnlyList<GenerationDto> Generations { get; init; } = [];

    public IReadOnlyList<VersionDto> Versions { get; init; } = [];

    public IReadOnlyList<EngineDto> Engines { get; init; } = [];

    public IReadOnlyList<VehicleBodyDto> VehicleBodies { get; init; } = [];

    public IReadOnlyList<VehicleSpecDto> VehicleSpecs { get; init; } = [];
}

internal sealed record ManufacturerDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required Manufacturer Manufacturer { get; init; }
}

internal sealed record ModelDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int ManufacturerId { get; init; }
}

internal sealed record GenerationDto
{
    public required int Id { get; init; }

    public required int ModelId { get; init; }

    public required string Name { get; init; }

    public required int StartYear { get; init; }

    public int? EndYear { get; init; }

    public required bool ContainsVersions { get; init; }
}

internal sealed record VersionDto
{
    public required int Id { get; init; }

    public required int GenerationId { get; init; }

    public required string Name { get; init; }

    public required int StartYear { get; init; }

    public int? EndYear { get; init; }

    public required BodyType BodyType { get; init; }
}

internal sealed record EngineDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public int? GenerationId { get; init; }

    public int? VersionId { get; init; }
}

internal sealed record VehicleBodyDto
{
    public required int VehicleBodyId { get; init; }

    public int? GenerationId { get; init; }

    public int? VersionId { get; init; }

    public BodyParametersDto BodyParameters { get; init; } = new();
}

internal sealed record VehicleSpecDto
{
    public required int VehicleSpecId { get; init; }

    public required int VehicleBodyId { get; init; }

    public int? EngineId { get; init; }

    public EngineSpecsDto EngineSpecs { get; init; } = new();

    public DrivetrainSpecsDto DrivetrainSpecs { get; init; } = new();

    public PerformanceSpecsDto PerformanceSpecs { get; init; } = new();
}

internal sealed record BodyParametersDto
{
    public BodyBasicParametersDto BasicParameters { get; init; } = new();

    public ExternalDimensionsDto ExternalDimensions { get; init; } = new();

    public TrunkDimensionsDto TrunkDimensions { get; init; } = new();
}

internal sealed record BodyBasicParametersDto
{
    public ParameterValueDto? NumberOfDoors { get; init; }

    public ParameterValueDto? NumberOfSeats { get; init; }

    public ParameterValueDto? TurningDiameter { get; init; }

    public ParameterValueDto? TurningRadius { get; init; }
}

internal sealed record ExternalDimensionsDto
{
    public ParameterValueDto? Length { get; init; }

    public ParameterValueDto? Width { get; init; }

    public ParameterValueDto? Height { get; init; }

    public ParameterValueDto? Wheelbase { get; init; }

    public ParameterValueDto? GroundClearance { get; init; }
}

internal sealed record TrunkDimensionsDto
{
    public ParameterValueDto? MaximumTrunkCapacitySeatsFolded { get; init; }

    public ParameterValueDto? MinimumTrunkCapacitySeatsUp { get; init; }
}

internal sealed record EngineSpecsDto
{
    public ParameterValueDto? Capacity { get; init; }

    public FuelType FuelType { get; init; } = FuelType.Unknown;

    public EngineArchitectureDto Architecture { get; init; } = new();

    public EnginePowerSpecsDto Power { get; init; } = new();

    public EngineTorqueSpecsDto Torque { get; init; } = new();
}

internal sealed record EngineArchitectureDto
{
    public ParameterValueDto? CylinderCount { get; init; }

    public string CylinderArrangement { get; init; } = string.Empty;

    public ParameterValueDto? ValveCount { get; init; }
}

internal sealed record EnginePowerSpecsDto
{
    public ParameterValueDto? Horsepower { get; init; }

    public ParameterValueDto? AtRpm { get; init; }
}

internal sealed record EngineTorqueSpecsDto
{
    public ParameterValueDto? MaxTorque { get; init; }

    public ParameterValueDto? AtRpmFrom { get; init; }

    public ParameterValueDto? AtRpmTo { get; init; }
}

internal sealed record DrivetrainSpecsDto
{
    public TransmissionType TransmissionType { get; init; } = TransmissionType.Unknown;

    public Drivetrain Drivetrain { get; init; } = Drivetrain.Unknown;
}

internal sealed record PerformanceSpecsDto
{
    public ParameterValueDto? Acceleration0To100 { get; init; }

    public ParameterValueDto? TopSpeed { get; init; }
}

internal sealed record ParameterValueDto
{
    public decimal? Value { get; init; }

    public MeasurementUnit Unit { get; init; } = MeasurementUnit.None;

    public bool IsMissing { get; init; }
}

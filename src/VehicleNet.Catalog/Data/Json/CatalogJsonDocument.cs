using System.Text.Json;
using VehicleNet.Common.Enums;

namespace VehicleNet.Catalog.Data.Json;

internal sealed record CatalogJsonDocument
{
    public IReadOnlyList<ManufacturerDto> Manufacturers { get; init; } = [];

    public IReadOnlyList<ModelDto> Models { get; init; } = [];

    public IReadOnlyList<GenerationDto> Generations { get; init; } = [];

    public IReadOnlyList<VersionDto> Versions { get; init; } = [];

    public IReadOnlyList<EngineDto> Engines { get; init; } = [];

    public IReadOnlyList<EngineVariantDto> EngineVariants { get; init; } = [];

    public IReadOnlyList<VehicleBodyDto> VehicleBodies { get; init; } = [];

    public IReadOnlyList<VehicleBodyEngineDto> VehicleBodyEngines { get; init; } = [];

    public IReadOnlyList<VehicleBodyEngineVariantDto> VehicleBodyEngineVariants { get; init; } = [];

    public void Validate()
    {
        ValidateUniqueIds(Manufacturers, m => m.Id, "Manufacturer");
        ValidateUniqueIds(Models, m => m.Id, "Model");
        ValidateUniqueIds(Generations, g => g.Id, "Generation");
        ValidateUniqueIds(Versions, v => v.Id, "Version");
        ValidateUniqueIds(Engines, e => e.Id, "Engine");
        ValidateUniqueIds(EngineVariants, ev => ev.Id, "EngineVariant");
        ValidateUniqueIds(VehicleBodies, vb => vb.Id, "VehicleBody");
        ValidateUniqueIds(VehicleBodyEngines, vbe => vbe.Id, "VehicleBodyEngine");
        ValidateUniqueIds(VehicleBodyEngineVariants, vbev => vbev.Id, "VehicleBodyEngineVariant");
    }

    private static void ValidateUniqueIds<T>(IReadOnlyList<T> items, Func<T, int> idSelector, string entityName)
    {
        var duplicates = items
            .GroupBy(idSelector)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Count > 0)
        {
            throw new InvalidOperationException(
                $"Duplicate {entityName} IDs found: {string.Join(", ", duplicates)}. Each {entityName} must have a unique ID.");
        }
    }
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

    public required int GId { get; init; }

    public required string Name { get; init; }

    public required int StartYear { get; init; }

    public int? EndYear { get; init; }

    public required BodyType BodyType { get; init; }
}

internal sealed record EngineDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public int? GId { get; init; }

    public int? VId { get; init; }
}

internal sealed record EngineVariantDto
{
    public required int Id { get; init; }

    public required int EId { get; init; }

    public required string Name { get; init; }
}

internal sealed record VehicleBodyDto
{
    public required int Id { get; init; }

    public int? GId { get; init; }

    public int? VId { get; init; }

    public BodySpecsDto BodySpecs { get; init; } = new();
}

internal sealed record VehicleBodyEngineDto
{
    public required int Id { get; init; }

    public required int VbId { get; init; }

    public int? EId { get; init; }

    public EngineSpecsDto EngineSpecs { get; init; } = new();
}

internal sealed record BodySpecsDto
{
    public BodyBasicParametersDto? BasicParameters { get; init; }

    public ExternalDimensionsDto? ExternalDimensions { get; init; }

    public TrunkDimensionsDto? TrunkDimensions { get; init; }
}

internal sealed record BodyBasicParametersDto
{
    public JsonElement? NumberOfDoors { get; init; }

    public JsonElement? NumberOfSeats { get; init; }

    public JsonElement? TurningDiameter { get; init; }

    public JsonElement? TurningRadius { get; init; }
}

internal sealed record ExternalDimensionsDto
{
    public JsonElement? Length { get; init; }

    public JsonElement? Width { get; init; }

    public JsonElement? Height { get; init; }

    public JsonElement? Wheelbase { get; init; }

    public JsonElement? GroundClearance { get; init; }
}

internal sealed record TrunkDimensionsDto
{
    public JsonElement? MaximumTrunkCapacitySeatsFolded { get; init; }

    public JsonElement? MinimumTrunkCapacitySeatsUp { get; init; }
}

internal sealed record EngineSpecsDto
{
    public JsonElement? Capacity { get; init; }

    public FuelType FuelType { get; init; } = FuelType.Unknown;

    public EngineArchitectureDto? Architecture { get; init; }

    public EnginePowerSpecsDto? Power { get; init; }

    public EngineTorqueSpecsDto? Torque { get; init; }
}

internal sealed record EngineArchitectureDto
{
    public JsonElement? CylinderCount { get; init; }

    public string CylinderArrangement { get; init; } = string.Empty;

    public JsonElement? ValveCount { get; init; }
}

internal sealed record EnginePowerSpecsDto
{
    public JsonElement? Horsepower { get; init; }

    public JsonElement? At { get; init; }
}

internal sealed record EngineTorqueSpecsDto
{
    public JsonElement? MaxTorque { get; init; }

    public JsonElement? From { get; init; }

    public JsonElement? To { get; init; }
}

internal sealed record DrivetrainSpecsDto
{
    public TransmissionType TransmissionType { get; init; } = TransmissionType.Unknown;

    public Drivetrain Drivetrain { get; init; } = Drivetrain.Unknown;
}

internal sealed record PerformanceSpecsDto
{
    public JsonElement? Acceleration0To100 { get; init; }

    public JsonElement? TopSpeed { get; init; }
}

internal sealed record EngineVariantSpecsDto
{
    public DrivetrainSpecsDto? DrivetrainSpecs { get; init; }

    public PerformanceSpecsDto? PerformanceSpecs { get; init; }
}

internal sealed record VehicleBodyEngineVariantDto
{
    public required int Id { get; init; }

    public required int VbeId { get; init; }

    public required int EvId { get; init; }

    public EngineVariantSpecsDto EngineVariantSpecs { get; init; } = new();
}


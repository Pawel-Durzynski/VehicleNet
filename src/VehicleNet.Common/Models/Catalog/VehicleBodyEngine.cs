using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog;

public sealed record VehicleBodyEngine
{
    public required int VehicleBodyEngineId { get; init; }

    public required int VehicleBodyId { get; init; }

    public VehicleBody? VehicleBody { get; init; }

    public int EngineId { get; init; }

    public required VehicleEngine Engine { get; init; }

    public int? GenerationId { get; init; }

    public VehicleGeneration? Generation { get; init; }

    public int? VersionId { get; init; }

    public VehicleVersion? Version { get; init; }

    public required EngineSpecs EngineSpecs { get; init; }

    protected VehicleBodyEngine(VehicleBodyEngine original)
    {
        VehicleBodyEngineId = original.VehicleBodyEngineId;
        VehicleBodyId = original.VehicleBodyId;
        VehicleBody = original.VehicleBody;
        GenerationId = original.GenerationId;
        Generation = original.Generation;
        VersionId = original.VersionId;
        Version = original.Version;
        EngineId = original.EngineId;
        Engine = original.Engine;
        EngineSpecs = original.EngineSpecs;
    }

    private void ValidateConsistency()
    {
        if (EngineId != Engine.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngine {VehicleBodyEngineId} has mismatched EngineId ({EngineId}) and Engine.Id ({Engine.Id}).");
        }

        if (VehicleBody is not null && VehicleBodyId != VehicleBody.VehicleBodyId)
        {
            throw new InvalidOperationException($"VehicleBodyEngine {VehicleBodyEngineId} has mismatched VehicleBodyId ({VehicleBodyId}) and VehicleBody.VehicleBodyId ({VehicleBody.VehicleBodyId}).");
        }

        if (GenerationId.HasValue && Generation is not null && GenerationId.Value != Generation.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngine {VehicleBodyEngineId} has mismatched GenerationId ({GenerationId}) and Generation.Id ({Generation.Id}).");
        }

        if (VersionId.HasValue && Version is not null && VersionId.Value != Version.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngine {VehicleBodyEngineId} has mismatched VersionId ({VersionId}) and Version.Id ({Version.Id}).");
        }

        if (Version is not null && Generation is null)
        {
            throw new InvalidOperationException($"VehicleBodyEngine {VehicleBodyEngineId} has Version set but Generation is null.");
        }

        if (Version is not null && Generation is not null && Version.GenerationId != Generation.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngine {VehicleBodyEngineId} has Version.GenerationId ({Version.GenerationId}) that does not match Generation.Id ({Generation.Id}).");
        }
    }

    public VehicleHierarchy Hierarchy
    {
        get
        {
            ValidateConsistency();

            return field ??= new(
                Generation.Model.Manufacturer.Name,
                Generation.Model.Name,
                Generation.Name,
                Version?.Name ?? string.Empty,
                Engine.Name,
                string.Empty);
        }
    }

    public string DisplayName
    {
        get
        {
            ValidateConsistency();

            return field ??= $"{Generation.Model.Manufacturer.Name} {Generation.Model.Name} {Generation.Name} {Version?.Name} {Engine.Name}".Trim();
        }
    }
}

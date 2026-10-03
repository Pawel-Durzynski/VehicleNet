using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog;

/// <summary>Associates a vehicle body with an engine and its specifications.</summary>
public sealed record VehicleBodyEngine
{
    /// <summary>Gets the body-engine association identifier.</summary>
    public required int VehicleBodyEngineId { get; init; }

    /// <summary>Gets the vehicle body identifier.</summary>
    public required int VehicleBodyId { get; init; }

    /// <summary>Gets the vehicle body.</summary>
    public VehicleBody? VehicleBody { get; init; }

    /// <summary>Gets the engine identifier.</summary>
    public int EngineId { get; init; }

    /// <summary>Gets the engine.</summary>
    public required VehicleEngine Engine { get; init; }

    /// <summary>Gets the optional generation identifier.</summary>
    public int? GenerationId { get; init; }

    /// <summary>Gets the optional generation.</summary>
    public VehicleGeneration? Generation { get; init; }

    /// <summary>Gets the optional version identifier.</summary>
    public int? VersionId { get; init; }

    /// <summary>Gets the optional version.</summary>
    public VehicleVersion? Version { get; init; }

    /// <summary>Gets the engine specifications.</summary>
    public required EngineSpecs EngineSpecs { get; init; }

    /// <summary>Initializes a copy of an existing body-engine association.</summary>
    /// <param name="original">The association to copy.</param>
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

    /// <summary>Gets the resolved catalog hierarchy.</summary>
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

    /// <summary>Gets the display name assembled from the catalog hierarchy.</summary>
    public string DisplayName
    {
        get
        {
            ValidateConsistency();

            return field ??= $"{Generation.Model.Manufacturer.Name} {Generation.Model.Name} {Generation.Name} {Version?.Name} {Engine.Name}".Trim();
        }
    }
}

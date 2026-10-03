using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog;

/// <summary>Associates a body-engine combination with a specific engine variant.</summary>
public sealed record VehicleBodyEngineVariant
{
    /// <summary>Gets the body-engine-variant association identifier.</summary>
    public required int VehicleBodyEngineVariantId { get; init; }

    /// <summary>Gets the body-engine association identifier.</summary>
    public required int VehicleBodyEngineId { get; init; }

    /// <summary>Gets the body-engine association.</summary>
    public required VehicleBodyEngine VehicleBodyEngine { get; init; }

    /// <summary>Gets the engine variant identifier.</summary>
    public required int EngineVariantId { get; init; }

    /// <summary>Gets the engine variant.</summary>
    public required EngineVariant EngineVariant { get; init; }

    /// <summary>Gets the optional generation identifier.</summary>
    public int? GenerationId { get; init; }

    /// <summary>Gets the optional generation.</summary>
    public VehicleGeneration? Generation { get; init; }

    /// <summary>Gets the optional version identifier.</summary>
    public int? VersionId { get; init; }

    /// <summary>Gets the optional version.</summary>
    public VehicleVersion? Version { get; init; }

    /// <summary>Gets the engine variant specifications.</summary>
    public required EngineVariantSpecs EngineVariantSpecs { get; init; }

    /// <summary>Initializes a copy of an existing body-engine-variant association.</summary>
    /// <param name="original">The association to copy.</param>
    protected VehicleBodyEngineVariant(VehicleBodyEngineVariant original)
    {
        VehicleBodyEngineVariantId = original.VehicleBodyEngineVariantId;
        GenerationId = original.GenerationId;
        VersionId = original.VersionId;
        VehicleBodyEngineId = original.VehicleBodyEngineId;
        EngineVariantId = original.EngineVariantId;
        EngineVariantSpecs = original.EngineVariantSpecs;
        VehicleBodyEngine = original.VehicleBodyEngine;
        Generation = original.Generation;
        Version = original.Version;
        EngineVariant = original.EngineVariant;
    }

    private void ValidateConsistency()
    {
        if (VehicleBodyEngineId != VehicleBodyEngine.VehicleBodyEngineId)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has mismatched VehicleBodyEngineId ({VehicleBodyEngineId}) and VehicleBodyEngine.VehicleBodyEngineId ({VehicleBodyEngine.VehicleBodyEngineId}).");
        }

        if (EngineVariantId != EngineVariant.EngineVariantId)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has mismatched EngineVariantId ({EngineVariantId}) and EngineVariant.EngineVariantId ({EngineVariant.EngineVariantId}).");
        }

        if (EngineVariant.EngineId != VehicleBodyEngine.EngineId)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has EngineVariant.EngineId ({EngineVariant.EngineId}) that does not match VehicleBodyEngine.EngineId ({VehicleBodyEngine.EngineId}).");
        }

        if (GenerationId.HasValue && Generation is not null && GenerationId.Value != Generation.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has mismatched GenerationId ({GenerationId}) and Generation.Id ({Generation.Id}).");
        }

        if (VersionId.HasValue && Version is not null && VersionId.Value != Version.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has mismatched VersionId ({VersionId}) and Version.Id ({Version.Id}).");
        }

        if (GenerationId.HasValue && VehicleBodyEngine.GenerationId.HasValue && GenerationId.Value != VehicleBodyEngine.GenerationId.Value)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has GenerationId ({GenerationId}) that does not match VehicleBodyEngine.GenerationId ({VehicleBodyEngine.GenerationId}).");
        }

        if (VersionId.HasValue && VehicleBodyEngine.VersionId.HasValue && VersionId.Value != VehicleBodyEngine.VersionId.Value)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has VersionId ({VersionId}) that does not match VehicleBodyEngine.VersionId ({VehicleBodyEngine.VersionId}).");
        }

        if (Generation is not null && VehicleBodyEngine.Generation is not null && Generation.Id != VehicleBodyEngine.Generation.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has Generation.Id ({Generation.Id}) that does not match VehicleBodyEngine.Generation.Id ({VehicleBodyEngine.Generation.Id}).");
        }

        if (Version is not null && VehicleBodyEngine.Version is not null && Version.Id != VehicleBodyEngine.Version.Id)
        {
            throw new InvalidOperationException($"VehicleBodyEngineVariant {VehicleBodyEngineVariantId} has Version.Id ({Version.Id}) that does not match VehicleBodyEngine.Version.Id ({VehicleBodyEngine.Version.Id}).");
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
                Version.Name,
                VehicleBodyEngine.Engine.Name,
                EngineVariant.Name);
        }
    }

    /// <summary>Gets the display name assembled from the catalog hierarchy.</summary>
    public string DisplayName
    {
        get
        {
            ValidateConsistency();

            return field ??= $"{Generation.Model.Manufacturer.Name} {Generation.Model.Name} {Generation.Name} {Version?.Name} {EngineVariant.Name}".Trim();
        }
    }
}

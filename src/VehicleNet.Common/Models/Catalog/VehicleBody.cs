using VehicleNet.Common.Models.Catalog.BodyDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog;

/// <summary>Represents a vehicle body and its specifications.</summary>
public sealed record VehicleBody
{
    /// <summary>Gets the vehicle body identifier.</summary>
    public required int VehicleBodyId { get; init; }

    /// <summary>Gets the optional generation identifier.</summary>
    public int? GenerationId { get; init; }

    /// <summary>Gets the optional generation.</summary>
    public VehicleGeneration? Generation { get; init; }

    /// <summary>Gets the optional version identifier.</summary>
    public int? VersionId { get; init; }

    /// <summary>Gets the optional version.</summary>
    public VehicleVersion? Version { get; init; }

    /// <summary>Gets the body specifications.</summary>
    public required BodySpecs BodySpecs { get; init; }

    /// <summary>Initializes a copy of an existing vehicle body.</summary>
    /// <param name="original">The vehicle body to copy.</param>
    protected VehicleBody(VehicleBody original)
    {
        VehicleBodyId = original.VehicleBodyId;
        GenerationId = original.GenerationId;
        Generation = original.Generation;
        VersionId = original.VersionId;
        Version = original.Version;
        BodySpecs = original.BodySpecs;
    }

    private void ValidateConsistency()
    {
        if (GenerationId.HasValue && Generation is not null && GenerationId.Value != Generation.Id)
        {
            throw new InvalidOperationException($"VehicleBody {VehicleBodyId} has mismatched GenerationId ({GenerationId}) and Generation.Id ({Generation.Id}).");
        }

        if (VersionId.HasValue && Version is not null && VersionId.Value != Version.Id)
        {
            throw new InvalidOperationException($"VehicleBody {VehicleBodyId} has mismatched VersionId ({VersionId}) and Version.Id ({Version.Id}).");
        }

        if (Version is not null && Generation is null)
        {
            throw new InvalidOperationException($"VehicleBody {VehicleBodyId} has Version set but Generation is null.");
        }

        if (Version is not null && Generation is not null && Version.GenerationId != Generation.Id)
        {
            throw new InvalidOperationException($"VehicleBody {VehicleBodyId} has Version.GenerationId ({Version.GenerationId}) that does not match Generation.Id ({Generation.Id}).");
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
                string.Empty,
                string.Empty);
        }
    }

    /// <summary>Gets the display name assembled from the catalog hierarchy.</summary>
    public string DisplayName
    {
        get
        {
            ValidateConsistency();

            return field ??= $"{Generation.Model.Manufacturer.Name} {Generation.Model.Name} {Generation.Name} {Version?.Name}".Trim();
        }
    }
}

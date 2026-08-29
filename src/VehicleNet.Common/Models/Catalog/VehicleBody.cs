using VehicleNet.Common.Models.Catalog.BodyDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog;

public sealed record VehicleBody
{
    public required int VehicleBodyId { get; init; }

    public int? GenerationId { get; init; }

    public VehicleGeneration? Generation { get; init; }

    public int? VersionId { get; init; }

    public VehicleVersion? Version { get; init; }

    public required BodySpecs BodySpecs { get; init; }

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

    public string DisplayName
    {
        get
        {
            ValidateConsistency();

            return field ??= $"{Generation.Model.Manufacturer.Name} {Generation.Model.Name} {Generation.Name} {Version?.Name}".Trim();
        }
    }
}

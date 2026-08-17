using VehicleNet.Common.Models.Catalog.BodyDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog;

public record VehicleBody
{
    public required int VehicleBodyId { get; init; }

    public int? GenerationId { get; init; }

    public VehicleGeneration? Generation { get; init; }

    public int? VersionId { get; init; }

    public VehicleVersion? Version { get; init; }

    public BodyParameters BodyParameters { get; init; }

    protected VehicleBody(VehicleBody original)
    {
        VehicleBodyId = original.VehicleBodyId;
        GenerationId = original.GenerationId;
        Generation = original.Generation;
        VersionId = original.VersionId;
        Version = original.Version;
        BodyParameters = original.BodyParameters;
        // _hierarchy and _displayName intentionally not copied — recomputed on first access
    }

    private VehicleHierarchy? _hierarchy;
    public virtual VehicleHierarchy Hierarchy => _hierarchy ??= new(
        Generation.Model.Manufacturer.Name,
        Generation.Model.Name,
        Generation.Name,
        Version?.Name ?? string.Empty,
        string.Empty);

    private string? _displayName;
    public virtual string DisplayName =>
        _displayName ??= $"{Generation.Model.Manufacturer.Name} {Generation.Model.Name} {Generation.Name} {Version?.Name}".Trim();
}

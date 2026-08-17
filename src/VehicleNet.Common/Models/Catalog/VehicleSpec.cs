using VehicleNet.Common.Models.Catalog.BodyDetails;
using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog;

public sealed record VehicleSpec
{
    public required int VehicleSpecId { get; init; }

    public required int VehicleBodyId { get; init; }

    public VehicleBody? VehicleBody { get; init; }

    public int? GenerationId { get; init; }

    public VehicleGeneration? Generation { get; init; }

    public int? VersionId { get; init; }

    public VehicleVersion? Version { get; init; }

    public int? EngineId { get; init; }

    public BodyParameters BodyParameters { get; init; }

    public VehicleEngine? Engine { get; init; }

    public required TechnicalSpecs TechnicalSpecs { get; init; }

    protected VehicleSpec(VehicleSpec original)
    {
        VehicleSpecId = original.VehicleSpecId;
        VehicleBodyId = original.VehicleBodyId;
        VehicleBody = original.VehicleBody;
        GenerationId = original.GenerationId;
        Generation = original.Generation;
        VersionId = original.VersionId;
        Version = original.Version;
        EngineId = original.EngineId;
        Engine = original.Engine;
        BodyParameters = original.BodyParameters;
        TechnicalSpecs = original.TechnicalSpecs;
        // _hierarchy and _displayName intentionally not copied — recomputed on first access
    }

    private VehicleHierarchy? _hierarchy;
    public VehicleHierarchy Hierarchy => _hierarchy ??= new(
        Generation.Model.Manufacturer.Name,
        Generation.Model.Name,
        Generation.Name,
        Version?.Name ?? string.Empty,
        Engine.Name);

    private string? _displayName;
    public string DisplayName =>
        _displayName ??= $"{Generation.Model.Manufacturer.Name} {Generation.Model.Name} {Generation.Name} {Version?.Name} {Engine.Name}".Trim();
}

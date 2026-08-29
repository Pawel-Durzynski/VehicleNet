using VehicleNet.Common.Models.Catalog.Hierarchy;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EngineVariant
{
    public required int EngineVariantId { get; init; }

    public required int EngineId { get; init; }

    public required string Name { get; init; }

    public required VehicleEngine Engine { get; init; }
}

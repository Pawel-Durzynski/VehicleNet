using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Catalog.Hierarchy;

public sealed record VehicleVersion(
    int Id,
    int GenerationId,
    string Name,
    int StartYear,
    int? EndYear,
    BodyType BodyType,
    VehicleGeneration Generation)
{
    public string DisplayName =>
        $"{Name} ({StartYear} - {(EndYear.HasValue ? EndYear.Value.ToString() : "present")})";
}

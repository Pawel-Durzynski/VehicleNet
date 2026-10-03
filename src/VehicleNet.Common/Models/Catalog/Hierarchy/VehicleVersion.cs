using VehicleNet.Common.Enums;

namespace VehicleNet.Common.Models.Catalog.Hierarchy;

/// <summary>Represents a vehicle version in the catalog hierarchy.</summary>
/// <param name="Id">The version identifier.</param>
/// <param name="GenerationId">The parent generation identifier.</param>
/// <param name="Name">The version name.</param>
/// <param name="StartYear">The first production year.</param>
/// <param name="EndYear">The optional final production year.</param>
/// <param name="BodyType">The vehicle body type.</param>
/// <param name="Generation">The parent generation.</param>
public sealed record VehicleVersion(
    int Id,
    int GenerationId,
    string Name,
    int StartYear,
    int? EndYear,
    BodyType BodyType,
    VehicleGeneration Generation)
{
    /// <summary>Gets the version name and production-year range.</summary>
    public string DisplayName =>
        $"{Name} ({StartYear} - {(EndYear.HasValue ? EndYear.Value.ToString() : "present")})";
}

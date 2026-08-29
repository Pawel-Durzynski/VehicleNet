using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

public sealed class VehicleBodyService : IVehicleBodyService
{
    private readonly IReadOnlyList<VehicleBody> _bodies;

    public VehicleBodyService(IEnumerable<VehicleBody> bodies)
    {
        _bodies = bodies.ToList();
    }

    public VehicleBodySearchResult Search(VehicleBodySearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IEnumerable<VehicleBody> query = _bodies;

        if (criteria.VehicleBodyId.HasValue)
        {
            query = query.Where(v => v.VehicleBodyId == criteria.VehicleBodyId.Value);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Manufacturer))
        {
            query = query.Where(v => MatchesText(v.Hierarchy.Manufacturer, criteria.Manufacturer));
        }

        if (!string.IsNullOrWhiteSpace(criteria.Model))
        {
            query = query.Where(v => MatchesText(v.Hierarchy.Model, criteria.Model));
        }

        if (criteria.GenerationId.HasValue)
        {
            query = query.Where(v => v.GenerationId == criteria.GenerationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Generation))
        {
            query = query.Where(v => MatchesText(v.Hierarchy.Generation, criteria.Generation));
        }

        if (criteria.VersionId.HasValue)
        {
            query = query.Where(v => v.VersionId == criteria.VersionId.Value);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Version))
        {
            query = query.Where(v => MatchesText(v.Hierarchy.Version, criteria.Version));
        }

        var filtered = query.ToList();

        return new VehicleBodySearchResult(filtered.Count, filtered);
    }

    private static bool MatchesText(string source, string? expected)
    {
        if (string.IsNullOrWhiteSpace(expected))
        {
            return true;
        }

        return source.Contains(expected, StringComparison.OrdinalIgnoreCase);
    }
}

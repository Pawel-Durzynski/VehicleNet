using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

public sealed class VehicleSpecService : IVehicleSpecService
{
    private readonly IReadOnlyList<VehicleSpec> _vehicles;

    public VehicleSpecService(IEnumerable<VehicleSpec> vehicles)
    {
        _vehicles = vehicles.ToList();
    }

    public VehicleSpecSearchResult Search(VehicleSpecSearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IEnumerable<VehicleSpec> query = _vehicles;

        if (criteria.EngineId.HasValue)
        {
            query = query.Where(v => v.EngineId == criteria.EngineId.Value);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Engine))
        {
            query = query.Where(v => MatchesText(v.Engine.Name, criteria.Engine));
        }

        if (!string.IsNullOrWhiteSpace(criteria.Manufacturer))
        {
            query = query.Where(v => MatchesText(v.Hierarchy.Manufacturer, criteria.Manufacturer));
        }

        if (!string.IsNullOrWhiteSpace(criteria.Model))
        {
            query = query.Where(v => MatchesText(v.Hierarchy.Model, criteria.Model));
        }

        if (criteria.VehicleSpecId.HasValue)
        {
            query = query.Where(v => v.VehicleSpecId == criteria.VehicleSpecId.Value);
        }

        if (criteria.VehicleBodyId.HasValue)
        {
            query = query.Where(v => v.VehicleBodyId == criteria.VehicleBodyId.Value);
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

        return new VehicleSpecSearchResult(filtered.Count, filtered);
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

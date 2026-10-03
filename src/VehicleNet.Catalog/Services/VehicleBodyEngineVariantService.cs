using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class VehicleBodyEngineVariantService : IVehicleBodyEngineVariantService
{
    private readonly IReadOnlyList<VehicleBodyEngineVariant> _specifications;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleBodyEngineVariantService"/> class with the specified collection of vehicle body-engine variant specifications.
    /// </summary>
    /// <param name="specifications">The collection of vehicle body-engine variant specifications.</param>
    public VehicleBodyEngineVariantService(IEnumerable<VehicleBodyEngineVariant> specifications)
    {
        _specifications = specifications.ToList();
    }

    /// <inheritdoc/>
    public VehicleBodyEngineVariantSearchResult Search(VehicleBodyEngineVariantSearch search, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(search);

        IEnumerable<VehicleBodyEngineVariant> query = _specifications;

        if (search.EngineVariantId.HasValue)
        {
            query = query.Where(spec => spec.EngineVariantId == search.EngineVariantId.Value);
        }

        if (search.VehicleBodyEngineId.HasValue)
        {
            query = query.Where(spec => spec.VehicleBodyEngineId == search.VehicleBodyEngineId.Value);
        }

        if (search.TransmissionType.HasValue)
        {
            query = query.Where(spec => spec.EngineVariantSpecs.DrivetrainSpecs?.TransmissionType == search.TransmissionType.Value);
        }

        if (search.Drivetrain.HasValue)
        {
            query = query.Where(spec => spec.EngineVariantSpecs.DrivetrainSpecs?.Drivetrain == search.Drivetrain.Value);
        }

        var filtered = query.ToList();

        return new VehicleBodyEngineVariantSearchResult(filtered.Count, filtered);
    }
}

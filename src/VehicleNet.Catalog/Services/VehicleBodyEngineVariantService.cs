using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class VehicleBodyEngineVariantService : IVehicleBodyEngineVariantService
{
    private readonly IReadOnlyList<VehicleBodyEngineVariant> _specifications;

    public VehicleBodyEngineVariantService(IEnumerable<VehicleBodyEngineVariant> specifications)
    {
        _specifications = specifications.ToList();
    }

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
            query = query.Where(spec => spec.EngineVariantSpecs.DrivetrainSpecs.TransmissionType == search.TransmissionType.Value);
        }

        if (search.Drivetrain.HasValue)
        {
            query = query.Where(spec => spec.EngineVariantSpecs.DrivetrainSpecs.Drivetrain == search.Drivetrain.Value);
        }

        var filtered = query.ToList();

        return new VehicleBodyEngineVariantSearchResult(filtered.Count, filtered);
    }
}

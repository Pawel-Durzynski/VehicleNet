using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog.EngineDetails;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Services;

internal sealed class EngineVariantService : IEngineVariantService
{
    private readonly IReadOnlyList<EngineVariantDto> _variants;
    private readonly IEngineService _engineService;

    public EngineVariantService(
        IEnumerable<EngineVariantDto> engineVariants,
        IEngineService engineService)
    {
        _variants = engineVariants.ToList();
        _engineService = engineService;
    }

    public IEnumerable<EngineVariant> Search(EngineVariantSearch search)
    {
        ArgumentNullException.ThrowIfNull(search);

        var query = _variants.AsEnumerable();

        if (search.EngineId.HasValue)
        {
            query = query.Where(variant => variant.EId == search.EngineId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.Name))
        {
            query = query.Where(variant =>
                variant.Name.Contains(search.Name, StringComparison.OrdinalIgnoreCase));
        }

        return query
            .Select(MapVariant)
            .OrderBy(variant => variant.Name)
            .ToList();
    }

    private EngineVariant MapVariant(EngineVariantDto dto)
    {
        var engines = _engineService.Search(new EngineSearch { });
        var engine = engines.FirstOrDefault(e => e.Id == dto.EId)
            ?? throw new InvalidOperationException($"Engine {dto.EId} was not found for variant {dto.Id}.");

        return new EngineVariant
        {
            EngineVariantId = dto.Id,
            EngineId = dto.EId,
            Name = dto.Name,
            Engine = engine
        };
    }
}



using VehicleNet.Common.Models.Vin;
using VehicleNet.Vin.Interfaces;

namespace VehicleNet.Vin.Services;

/// <inheritdoc/>
public sealed class VinParserService : IVinParser
{
    private readonly IVinValidator _validator;
    private static readonly Dictionary<int, char> ModelYearCodeByYear = BuildModelYearCodeMap();

    /// <summary>
    /// Initializes a new instance of the <see cref="VinParserService"/> class with a default VIN validator.
    /// </summary>
    public VinParserService() : this(new VinValidatorService())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VinParserService"/> class with the specified VIN validator.
    /// </summary>
    /// <param name="validator">The VIN validator to use for validating VINs.</param>
    public VinParserService(IVinValidator validator)
    {
        _validator = validator;
    }

    /// <inheritdoc/>
    public VinParts Parse(string vin)
    {
        var validation = _validator.Validate(vin);
        if (!validation.IsValid)
        {
            throw new FormatException(validation.Error);
        }

        var normalized = vin.Trim().ToUpperInvariant();
        var yearCode = normalized[9];

        return new VinParts(
            normalized,
            normalized[..3],
            normalized.Substring(3, 6),
            normalized.Substring(9, 8),
            normalized[8],
            yearCode,
            DecodeModelYear(yearCode),
            normalized[10],
            normalized.Substring(11, 6));
    }

    private static int? DecodeModelYear(char yearCode)
    {
        yearCode = char.ToUpperInvariant(yearCode);

        var candidates = ModelYearCodeByYear
            .Where(x => x.Value == yearCode)
            .Select(x => x.Key)
            .OrderBy(x => x)
            .ToArray();

        if (candidates.Length == 0)
        {
            return null;
        }

        var reference = DateTime.UtcNow.Year + 1;
        var candidate = candidates.LastOrDefault(x => x <= reference);
        return candidate == 0 ? candidates[0] : candidate;
    }

    private static Dictionary<int, char> BuildModelYearCodeMap()
    {
        var sequence = "ABCDEFGHJKLMNPRSTVWXY123456789".ToCharArray();
        var map = new Dictionary<int, char>(60);

        for (var year = 1980; year <= 2039; year++)
        {
            map[year] = sequence[(year - 1980) % sequence.Length];
        }

        return map;
    }
}

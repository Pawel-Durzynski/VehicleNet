using VehicleNet.Common.Consts;
using VehicleNet.Common.Models.Vin;
using VehicleNet.Vin.Interfaces;

namespace VehicleNet.Vin.Services;

public sealed class VinGeneratorService : IVinGenerator
{
    private static readonly char[] AllowedVinChars = "ABCDEFGHJKLMNPRSTUVWXYZ0123456789".ToCharArray();
    private static readonly char[] AllowedWmiChars = "ABCDEFGHJKLMNPRSTUVWXYZ0123456789".ToCharArray();
    private static readonly char[] AllowedAlphaNumericWithoutAmbiguous = "ABCDEFGHJKLMNPRSTUVWXYZ0123456789".ToCharArray();
    private static readonly char[] Digits = "0123456789".ToCharArray();
    private static readonly Dictionary<int, char> ModelYearCodeByYear = BuildModelYearCodeMap();

    public string GenerateMockVin(VinGenerationOptions? options = null, Random? random = null)
    {
        random ??= Random.Shared;
        options ??= new VinGenerationOptions();

        var wmi = NormalizeWmi(options.WorldManufacturerIdentifier, random);

        var body = new char[17];
        body[0] = wmi[0];
        body[1] = wmi[1];
        body[2] = wmi[2];

        for (var i = 3; i <= 7; i++)
        {
            body[i] = Pick(AllowedAlphaNumericWithoutAmbiguous, random);
        }

        body[8] = '0';

        var year = options.ModelYear ?? DateTime.UtcNow.Year;
        body[9] = EncodeModelYear(year);
        body[10] = NormalizePlantCode(options.PlantCode, random);

        for (var i = 11; i <= 16; i++)
        {
            body[i] = Pick(Digits, random);
        }

        var vin = new string(body);
        body[8] = CalculateCheckDigit(vin);

        return new string(body);
    }

    private static char CalculateCheckDigit(string vin)
    {
        var sum = 0;

        for (var i = 0; i < vin.Length; i++)
        {
            var character = char.ToUpperInvariant(vin[i]);
            var value = VinConsts.Transliteration[character];
            sum += value * VinConsts.Weights[i];
        }

        var remainder = sum % 11;
        return remainder == 10 ? 'X' : (char)('0' + remainder);
    }

    private static string NormalizeWmi(string? wmi, Random random)
    {
        if (string.IsNullOrWhiteSpace(wmi))
        {
            return string.Create(3, random, (span, rng) =>
            {
                span[0] = Pick(AllowedWmiChars, rng);
                span[1] = Pick(AllowedWmiChars, rng);
                span[2] = Pick(AllowedWmiChars, rng);
            });
        }

        var normalized = wmi.Trim().ToUpperInvariant();
        if (normalized.Length != 3)
        {
            throw new ArgumentException("WMI must be exactly 3 characters.", nameof(wmi));
        }

        foreach (var c in normalized)
        {
            if (!AllowedVinChars.Contains(c) || c is 'I' or 'O' or 'Q')
            {
                throw new ArgumentException("WMI contains invalid characters.", nameof(wmi));
            }
        }

        return normalized;
    }

    private static char NormalizePlantCode(char? code, Random random)
    {
        if (!code.HasValue)
        {
            return Pick(AllowedAlphaNumericWithoutAmbiguous, random);
        }

        var normalized = char.ToUpperInvariant(code.Value);
        if (!AllowedVinChars.Contains(normalized) || normalized is 'I' or 'O' or 'Q')
        {
            throw new ArgumentException("Plant code must be an alphanumeric VIN-safe character.", nameof(code));
        }

        return normalized;
    }

    private static char EncodeModelYear(int year)
    {
        if (!ModelYearCodeByYear.TryGetValue(year, out var code))
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Model year must be between 1980 and 2039.");
        }

        return code;
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

    private static char Pick(IReadOnlyList<char> source, Random random)
    {
        return source[random.Next(source.Count)];
    }
}

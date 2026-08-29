using VehicleNet.Common.Models.Vin;
using VehicleNet.Vin.Interfaces;

namespace VehicleNet.ConsoleTest;

public class VehicleNetVin
{
    private readonly IVinValidator vinValidator;
    private readonly IVinParser vinParser;
    private readonly IVinGenerator vinGenerator;

    public VehicleNetVin(
        IVinValidator vinValidator,
        IVinParser vinParser,
        IVinGenerator vinGenerator)
    {
        this.vinValidator = vinValidator;
        this.vinParser = vinParser;
        this.vinGenerator = vinGenerator;
    }

    public void Playground()
    {
        Console.WriteLine("VIN demo scenarios");

        var generatedVin = vinGenerator.GenerateMockVin(new VinGenerationOptions
        {
            WorldManufacturerIdentifier = "WAU",
            ModelYear = DateTime.UtcNow.Year,
            PlantCode = 'A'
        });

        Console.WriteLine($"Generated VIN: {generatedVin}");

        var generatedValidation = vinValidator.Validate(generatedVin);
        Console.WriteLine($"Generated VIN valid: {generatedValidation.IsValid}");

        var parsed = vinParser.Parse(generatedVin);
        Console.WriteLine("Parsed generated VIN:");
        Console.WriteLine($"  WMI: {parsed.WorldManufacturerIdentifier}");
        Console.WriteLine($"  VDS: {parsed.VehicleDescriptorSection}");
        Console.WriteLine($"  VIS: {parsed.VehicleIdentifierSection}");
        Console.WriteLine($"  Check Digit: {parsed.CheckDigit}");
        Console.WriteLine($"  Model Year Code: {parsed.ModelYearCode}");
        Console.WriteLine($"  Model Year: {(parsed.ModelYear.HasValue ? parsed.ModelYear.Value : "N/A")}");
        Console.WriteLine($"  Plant Code: {parsed.PlantCode}");
        Console.WriteLine($"  Sequential Number: {parsed.SequentialNumber}");

        var invalidVin = generatedVin[..^1] + (generatedVin[^1] == '0' ? '1' : '0');
        var invalidValidation = vinValidator.Validate(invalidVin);
        Console.WriteLine($"Invalid VIN sample: {invalidVin}");
        Console.WriteLine($"Invalid VIN valid: {invalidValidation.IsValid}");
        if (!invalidValidation.IsValid)
        {
            Console.WriteLine($"Validation error: {invalidValidation.Error}");
        }

        var staticCheck = vinValidator.IsValid(generatedVin);
        Console.WriteLine($"IsValid shortcut for generated VIN: {staticCheck}");
    }
}

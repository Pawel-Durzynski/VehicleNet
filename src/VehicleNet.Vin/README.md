# VehicleNet.VIN

`VehicleNet.VIN` is a lightweight .NET library for:

- validating VINs
- parsing VIN parts
- generating algorithmically correct mock VINs (including valid checksum)

## Install

```bash
dotnet add package VehicleNet.VIN
```

## Features

- `IVinValidator`
  - `Validate(string?)`
  - `IsValid(string?)`
- `IVinParser`
  - `Parse(string)`
- `IVinGenerator`
  - `GenerateMockVin(VinGenerationOptions?, Random?)`

## Dependency Injection

Register VIN services:

```csharp
using Microsoft.Extensions.DependencyInjection;
using VehicleNet.Vin.Extensions;

var services = new ServiceCollection();
services.AddVehicleVinServices();

using var provider = services.BuildServiceProvider();
```

Resolve and use:

```csharp
using VehicleNet.Common.Models.Vin;
using VehicleNet.Vin.Interfaces;

var validator = provider.GetRequiredService<IVinValidator>();
var parser = provider.GetRequiredService<IVinParser>();
var generator = provider.GetRequiredService<IVinGenerator>();

// Generate VIN with options
var vin = generator.GenerateMockVin(new VinGenerationOptions
{
    WorldManufacturerIdentifier = "WAU",
    ModelYear = 2026,
    PlantCode = 'A'
});

var validation = validator.Validate(vin);
if (!validation.IsValid)
{
    throw new InvalidOperationException(validation.Error);
}

var parsed = parser.Parse(vin);
Console.WriteLine("Parsed generated VIN:");
Console.WriteLine($"  WMI: {parsed.WorldManufacturerIdentifier}");
Console.WriteLine($"  VDS: {parsed.VehicleDescriptorSection}");
Console.WriteLine($"  VIS: {parsed.VehicleIdentifierSection}");
Console.WriteLine($"  Check Digit: {parsed.CheckDigit}");
Console.WriteLine($"  Model Year Code: {parsed.ModelYearCode}");
Console.WriteLine($"  Model Year: {(parsed.ModelYear.HasValue ? parsed.ModelYear.Value : "N/A")}");
Console.WriteLine($"  Plant Code: {parsed.PlantCode}");
Console.WriteLine($"  Sequential Number: {parsed.SequentialNumber}");
```

Generate VIN without options:

```csharp
var randomVin = generator.GenerateMockVin();
var isValid = validator.IsValid(randomVin); // should be true
```

## VIN rules covered

- 17-character VIN length
- Disallowed ambiguous letters (`I`, `O`, `Q`)
- Transliteration and weighted checksum validation
- Position 9 check digit support (`0-9` and `X`)

## Models (from VehicleNet.Common)

- `VinParts`
- `VinValidationResult`
- `VinGenerationOptions`

## Target framework

- .NET 10 (`net10.0`)

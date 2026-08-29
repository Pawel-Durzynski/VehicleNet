# VehicleNet

VehicleNet is a modular, high-performance .NET library for automotive data. It provides robust tools for VIN (Vehicle Identification Number) validation, random VIN generation for testing purposes, and a comprehensive catalog of vehicle manufacturers, models, and specifications.

## Features

VehicleNet is designed as a monorepo containing modular NuGet packages, allowing you to include only the dependencies you actually need:

*   **`VehicleNet.Vin`**: A lightweight library for validating, parsing, and generating algoritmically correct mock VINs.
*   **`VehicleNet.Catalog`**: A comprehensive vehicle database containing manufacturers, models, generations, and basic specifications.

## Installation

Install the packages via the .NET CLI:

```bash
dotnet add package VehicleNet.Vin
dotnet add package VehicleNet.Catalog
```

## Architecture

This project uses a monorepo approach with shared MSBuild properties (`Directory.Build.props`) to ensure consistent versioning and metadata across all published packages.

## Contributing

Contributions are always welcome! Whether it's adding new models to the catalog, improving the VIN decoding algorithms, or fixing bugs:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Please ensure that your code is covered by unit tests before submitting a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Quick start for `VehicleNet.Catalog`

Register services:

```csharp
using Microsoft.Extensions.DependencyInjection;
using VehicleNet.Catalog.Extensions;

var services = new ServiceCollection();
services.AddVehicleCatalogServices();

using var provider = services.BuildServiceProvider();
```

Example: search until you get engine version spec:

```csharp
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Search;

var manufacturerService = provider.GetRequiredService<IManufacturerService>();
var modelService = provider.GetRequiredService<IModelService>();
var generationService = provider.GetRequiredService<IGenerationService>();
var versionService = provider.GetRequiredService<IVersionService>();
var engineService = provider.GetRequiredService<IEngineService>();
var engineVariantService = provider.GetRequiredService<IEngineVariantService>();
var vehicleBodyEngineVariantService = provider.GetRequiredService<IVehicleBodyEngineVariantService>();

var manufacturer = manufacturerService.Search(new ManufacturerSearch { Name = "Skoda" }).First();
var model = modelService.Search(new ModelSearch { ManufacturerId = manufacturer.Id, Name = "Octavia" }).First();
var generation = generationService.Search(new GenerationSearch { ModelId = model.Id, Name = "IV" }).First();

var selectedVersion = generation.ContainsVersions
    ? versionService.Search(new VersionSearch { GenerationId = generation.Id, Name = "RS" }).First()
    : null;

var engine = generation.ContainsVersions
    ? engineService.Search(new EngineSearch
    {
        VersionId = selectedVersion!.Id,
        Name = "2.0 TSI"
    }).First()
    : engineService.Search(new EngineSearch
    {
        GenerationId = generation.Id,
        Name = "2.0 TSI"
    }).First();

var engineVariant = engineVariantService.Search(new EngineVariantSearch
{
    EngineId = engine.Id,
    Name = "2.0 TSI Manual"
}).First();

var engineVersionSpec = vehicleBodyEngineVariantService.Search(
    new VehicleBodyEngineVariantSearch
    {
        EngineVariantId = engineVariant.EngineVariantId
    })
    .Items
    .First();

// Engine version spec:
var variantSpecs = engineVersionSpec.EngineVariantSpecs; // transmission, drivetrain, performance

// Engine spec:
var engineSpecs = engineVersionSpec.VehicleBodyEngine.EngineSpecs; // capacity, fuel, power, torque, architecture

// Body spec:
var bodySpecs = engineVersionSpec.VehicleBodyEngine.VehicleBody?.BodySpecs; // doors, seats, dimensions, trunk
```

## Quick start for `VehicleNet.Vin`

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
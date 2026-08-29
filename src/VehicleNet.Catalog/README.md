# VehicleNet.Catalog

`VehicleNet.Catalog` provides a ready-to-use vehicle catalog with manufacturers, models, generations, versions, engines, body specs, and engine variant specs.

## Features

- In-memory catalog services for:
  - manufacturers
  - models
  - generations
  - versions
  - engines
  - engine variants
  - vehicle body specifications
  - engine and engine-version specifications
- Search models using simple search DTOs.
- Easy DI registration for .NET applications.

## Install

```bash
dotnet add package VehicleNet.Catalog
```

## Quick start

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

## Available service interfaces

- `IManufacturerService`
- `IModelService`
- `IGenerationService`
- `IVersionService`
- `IEngineService`
- `IEngineVariantService`
- `IVehicleBodyService`
- `IVehicleBodyEngineService`
- `IVehicleBodyEngineVariantService`

## Target framework

- .NET 10 (`net10.0`)

## Notes

- Data is loaded from packaged resources.
- This package is intended for catalog querying scenarios and can be composed with other `VehicleNet.*` packages.

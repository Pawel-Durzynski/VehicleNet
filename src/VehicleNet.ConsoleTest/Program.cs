using Microsoft.Extensions.DependencyInjection;
using VehicleNet.Catalog.Extensions;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Search;
using VehicleNet.Common.Models.Units;

var services = new ServiceCollection();
services.AddVehicleCatalogServices();

using var serviceProvider = services.BuildServiceProvider();
var manufacturerService = serviceProvider.GetRequiredService<IManufacturerService>();
var modelService = serviceProvider.GetRequiredService<IModelService>();
var generationService = serviceProvider.GetRequiredService<IGenerationService>();
var versionService = serviceProvider.GetRequiredService<IVersionService>();
var engineService = serviceProvider.GetRequiredService<IEngineService>();
var bodyService = serviceProvider.GetRequiredService<IVehicleBodyService>();
var vehicleSpecService = serviceProvider.GetRequiredService<IVehicleSpecService>();

var manufacturers = manufacturerService.Search(new ManufacturerSearch { Name = "Skoda" });
var manufacturer = manufacturers.First();
var models = modelService.Search(new ModelSearch { ManufacturerId = manufacturer.Id, Name = "Octavia" });
var model = models.First();
var generations = generationService.Search(new GenerationSearch { ModelId = model.Id, Name = "IV" });
var generation = generations.First();
if (generation.ContainsVersions)
{
    var versions = versionService.Search(new VersionSearch { GenerationId = generation.Id, Name = "RS" });
    var version = versions.First();

    var bodySearch = new VehicleBodySearchCriteriaBuilder()
        .WithVersionId(version.Id)
        .Build();
    var bodyResult = bodyService.Search(bodySearch);
    PrintBodyItems(bodyResult.Items);

    var engines = engineService.Search(new EngineSearch { VersionId = version.Id, Name = "2.0 TSI" });
    var engine = engines.First();
    var vehicleSpecSearch = new VehicleSpecSearchCriteriaBuilder()
        .WithEngineId(engine.Id)
        .Build();
    var vehicleSpecResult = vehicleSpecService.Search(vehicleSpecSearch);
    PrintVehicleItems(vehicleSpecResult.Items);
}
else
{
    var bodySearch = new VehicleBodySearchCriteriaBuilder()
        .WithVersionId(generation.Id)
        .Build();
    var bodyResult = bodyService.Search(bodySearch);
    PrintBodyItems(bodyResult.Items);

    var engines = engineService.Search(new EngineSearch { GenerationId = generation.Id, Name = "RS" });
    var engine = engines.First();
    var vehicleSpecSearch = new VehicleSpecSearchCriteriaBuilder()
        .WithEngineId(engine.Id)
        .Build();
    var vehicleSpecResult = vehicleSpecService.Search(vehicleSpecSearch);
    PrintVehicleItems(vehicleSpecResult.Items);
}

static void PrintBodyItems(IEnumerable<VehicleBody> items)
{
    foreach (var item in items)
    {
        Console.WriteLine(item.DisplayName);
        Console.WriteLine($"  Vehicle Body Id: {item.VehicleBodyId}");
        Console.WriteLine($"  Generation Id: {(item.GenerationId.HasValue ? item.GenerationId.Value : "N/A")}");
        Console.WriteLine($"  Version Id: {(item.VersionId.HasValue ? item.VersionId.Value : "N/A")}");

        Console.WriteLine("  Body Parameters:");
        Console.WriteLine("    Basic Parameters:");
        Console.WriteLine($"      Number Of Doors: {Format(item.BodyParameters.BasicParameters.NumberOfDoors)}");
        Console.WriteLine($"      Number Of Seats: {Format(item.BodyParameters.BasicParameters.NumberOfSeats)}");
        Console.WriteLine($"      Turning Diameter: {Format(item.BodyParameters.BasicParameters.TurningDiameter)}");
        Console.WriteLine($"      Turning Radius: {Format(item.BodyParameters.BasicParameters.TurningRadius)}");

        Console.WriteLine("    External Dimensions:");
        Console.WriteLine($"      Length: {Format(item.BodyParameters.ExternalDimensions.Length)}");
        Console.WriteLine($"      Width: {Format(item.BodyParameters.ExternalDimensions.Width)}");
        Console.WriteLine($"      Height: {Format(item.BodyParameters.ExternalDimensions.Height)}");
        Console.WriteLine($"      Wheelbase: {Format(item.BodyParameters.ExternalDimensions.Wheelbase)}");
        Console.WriteLine($"      Ground Clearance: {Format(item.BodyParameters.ExternalDimensions.GroundClearance)}");

        Console.WriteLine("    Trunk Dimensions:");
        Console.WriteLine($"      Maximum Trunk Capacity Seats Folded: {Format(item.BodyParameters.TrunkDimensions.MaximumTrunkCapacitySeatsFolded)}");
        Console.WriteLine($"      Minimum Trunk Capacity Seats Up: {Format(item.BodyParameters.TrunkDimensions.MinimumTrunkCapacitySeatsUp)}");
        Console.WriteLine();
    }

    Console.WriteLine();
}

static void PrintVehicleItems(IEnumerable<VehicleSpec> items)
{
    foreach (var item in items)
    {
        Console.WriteLine(item.DisplayName);
        Console.WriteLine($"  Engine: {item.Engine?.Name ?? "N/A"}");

        Console.WriteLine("  Body Parameters:");
        Console.WriteLine("    Basic Parameters:");
        Console.WriteLine($"      Number Of Doors: {Format(item.BodyParameters.BasicParameters.NumberOfDoors)}");
        Console.WriteLine($"      Number Of Seats: {Format(item.BodyParameters.BasicParameters.NumberOfSeats)}");
        Console.WriteLine($"      Turning Diameter: {Format(item.BodyParameters.BasicParameters.TurningDiameter)}");
        Console.WriteLine($"      Turning Radius: {Format(item.BodyParameters.BasicParameters.TurningRadius)}");
        Console.WriteLine("    External Dimensions:");
        Console.WriteLine($"      Length: {Format(item.BodyParameters.ExternalDimensions.Length)}");
        Console.WriteLine($"      Width: {Format(item.BodyParameters.ExternalDimensions.Width)}");
        Console.WriteLine($"      Height: {Format(item.BodyParameters.ExternalDimensions.Height)}");
        Console.WriteLine($"      Wheelbase: {Format(item.BodyParameters.ExternalDimensions.Wheelbase)}");
        Console.WriteLine($"      Ground Clearance: {Format(item.BodyParameters.ExternalDimensions.GroundClearance)}");
        Console.WriteLine("    Trunk Dimensions:");
        Console.WriteLine($"      Maximum Trunk Capacity Seats Folded: {Format(item.BodyParameters.TrunkDimensions.MaximumTrunkCapacitySeatsFolded)}");
        Console.WriteLine($"      Minimum Trunk Capacity Seats Up: {Format(item.BodyParameters.TrunkDimensions.MinimumTrunkCapacitySeatsUp)}");

        Console.WriteLine("  Engine Specs:");
        Console.WriteLine($"    Capacity: {Format(item.TechnicalSpecs.EngineSpecs.Capacity)}");
        Console.WriteLine($"    Fuel Type: {item.TechnicalSpecs.EngineSpecs.FuelType}");
        Console.WriteLine("    Architecture:");
        Console.WriteLine($"      Cylinder Count: {Format(item.TechnicalSpecs.EngineSpecs.Architecture.CylinderCount)}");
        Console.WriteLine($"      Cylinder Arrangement: {item.TechnicalSpecs.EngineSpecs.Architecture.CylinderArrangement}");
        Console.WriteLine($"      Valve Count: {Format(item.TechnicalSpecs.EngineSpecs.Architecture.ValveCount)}");
        Console.WriteLine("    Power:");
        Console.WriteLine($"      Horsepower: {Format(item.TechnicalSpecs.EngineSpecs.Power.Horsepower)}");
        Console.WriteLine($"      At RPM: {Format(item.TechnicalSpecs.EngineSpecs.Power.AtRpm)}");
        Console.WriteLine("    Torque:");
        Console.WriteLine($"      Max Torque: {Format(item.TechnicalSpecs.EngineSpecs.Torque.MaxTorque)}");
        Console.WriteLine($"      At RPM From: {Format(item.TechnicalSpecs.EngineSpecs.Torque.AtRpmFrom)}");
        Console.WriteLine($"      At RPM To: {Format(item.TechnicalSpecs.EngineSpecs.Torque.AtRpmTo)}");

        Console.WriteLine("  Drivetrain Specs:");
        Console.WriteLine($"    Transmission Type: {item.TechnicalSpecs.DrivetrainSpecs.TransmissionType}");
        Console.WriteLine($"    Drivetrain: {item.TechnicalSpecs.DrivetrainSpecs.Drivetrain}");

        Console.WriteLine("  Performance Specs:");
        Console.WriteLine($"    0-100 km/h: {Format(item.TechnicalSpecs.PerformanceSpecs.Acceleration0To100)}");
        Console.WriteLine($"    Top Speed: {Format(item.TechnicalSpecs.PerformanceSpecs.TopSpeed)}");
        Console.WriteLine();
    }

    Console.WriteLine();
}

static string Format(ParameterValue value)
{
    if (!value.HasValue)
    {
        return "Missing";
    }

    return value.Unit.HasValue
        ? $"{value.Value} {value.Unit.Value}"
        : value.Value?.ToString() ?? "Missing";
}

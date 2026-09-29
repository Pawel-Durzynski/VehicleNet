using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Search;
using VehicleNet.Common.Models.Units;

namespace VehicleNet.ConsoleTest;

internal class VehicleNetCatalog
{
    private readonly IManufacturerService manufacturerService;
    private readonly IModelService modelService;
    private readonly IGenerationService generationService;
    private readonly IVersionService versionService;
    private readonly IEngineService engineService;
    private readonly IVehicleBodyService bodyService;
    private readonly IVehicleBodyEngineService vehicleBodyEngineService;
    private readonly IEngineVariantService engineVariantService;
    private readonly IVehicleBodyEngineVariantService vehicleBodyEngineVariantService;

    public VehicleNetCatalog(
        IManufacturerService manufacturerService,
        IModelService modelService,
        IGenerationService generationService,
        IVersionService versionService,
        IEngineService engineService,
        IVehicleBodyService bodyService,
        IVehicleBodyEngineService vehicleBodyEngineService,
        IEngineVariantService engineVariantService,
        IVehicleBodyEngineVariantService vehicleBodyEngineVariantService)
    {
        this.manufacturerService = manufacturerService;
        this.modelService = modelService;
        this.generationService = generationService;
        this.versionService = versionService;
        this.engineService = engineService;
        this.bodyService = bodyService;
        this.vehicleBodyEngineService = vehicleBodyEngineService;
        this.engineVariantService = engineVariantService;
        this.vehicleBodyEngineVariantService = vehicleBodyEngineVariantService;
    }

    public void Playground()
    {
        var manufacturers = manufacturerService.Search(new ManufacturerSearch { Name = "Skoda" });
        var manufacturer = manufacturers.First();
        var models = modelService.Search(new ModelSearch { ManufacturerId = manufacturer.Id, Name = "Octavia" });
        var model = models.First();
        var generations = generationService.Search(new GenerationSearch { ModelId = model.Id, Name = "I" });
        var generation = generations.First();
        if (generation.ContainsVersions)
        {
            var versions = versionService.Search(new VersionSearch { GenerationId = generation.Id, Name = "Kombi" });
            var version = versions.First();

            var bodySearch = new VehicleBodySearchCriteriaBuilder()
                .WithVersionId(version.Id)
                .Build();
            var bodyResult = bodyService.Search(bodySearch);
            PrintVehicleBodyItems(bodyResult.Items);

            var engines = engineService.Search(new EngineSearch { VersionId = version.Id, Name = "1.4 MPI 75KM" });
            var engine = engines.First();
            var vehicleBodyEngineSearch = new VehicleBodyEngineSearchCriteriaBuilder()
                .WithEngineId(engine.Id)
                .Build();
            var vehicleBodyEngineResult = vehicleBodyEngineService.Search(vehicleBodyEngineSearch);
            PrintVehicleBodyEngineItems(vehicleBodyEngineResult.Items);

            var engineVariants = engineVariantService.Search(new EngineVariantSearch { EngineId = engine.Id, Name = "1.4 MPI 75KM Manual" });
            var engineVariant = engineVariants.FirstOrDefault();

            var vehicleBodyEngineVariantSearch = new VehicleBodyEngineVariantSearchCriteriaBuilder()
                .WithEngineVariantId(engineVariant?.EngineVariantId)
                .Build();
            var vehicleBodyEngineVariantResult = vehicleBodyEngineVariantService.Search(vehicleBodyEngineVariantSearch);
            PrintVehicleBodyEngineVariantItems(vehicleBodyEngineVariantResult.Items);
        }
        else
        {
            var bodySearch = new VehicleBodySearchCriteriaBuilder()
                .WithVersionId(generation.Id)
                .Build();
            var bodyResult = bodyService.Search(bodySearch);
            PrintVehicleBodyItems(bodyResult.Items);

            var engines = engineService.Search(new EngineSearch { GenerationId = generation.Id, Name = "1.4 MPI 75KM" });
            var engine = engines.First();
            var vehicleBodyEngineSearch = new VehicleBodyEngineSearchCriteriaBuilder()
                .WithEngineId(engine.Id)
                .Build();
            var vehicleBodyEngineResult = vehicleBodyEngineService.Search(vehicleBodyEngineSearch);
            PrintVehicleBodyEngineItems(vehicleBodyEngineResult.Items);

            var engineVariants = engineVariantService.Search(new EngineVariantSearch { EngineId = engine.Id, Name = "1.4 MPI 75KM Manual" });
            var engineVariant = engineVariants.FirstOrDefault();

            var vehicleBodyEngineVariantSearch = new VehicleBodyEngineVariantSearchCriteriaBuilder()
                .WithEngineVariantId(engineVariant?.EngineVariantId)
                .Build();
            var vehicleBodyEngineVariantResult = vehicleBodyEngineVariantService.Search(vehicleBodyEngineVariantSearch);
            PrintVehicleBodyEngineVariantItems(vehicleBodyEngineVariantResult.Items);
        }
    }

    static void PrintVehicleBodyItems(IEnumerable<VehicleBody> items)
    {
        foreach (var item in items)
        {
            Console.WriteLine(item.DisplayName);
            Console.WriteLine($"  Vehicle Body Id: {item.VehicleBodyId}");
            Console.WriteLine($"  Generation Id: {(item.GenerationId.HasValue ? item.GenerationId.Value : "N/A")}");
            Console.WriteLine($"  Version Id: {(item.VersionId.HasValue ? item.VersionId.Value : "N/A")}");

            Console.WriteLine("  Body Specs:");
            Console.WriteLine("    Basic Parameters:");
            Console.WriteLine($"      Number Of Doors: {(item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.NumberOfDoors))}");
            Console.WriteLine($"      Number Of Seats: {(item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.NumberOfSeats))}");
            Console.WriteLine($"      Turning Diameter: {(item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.TurningDiameter))}");
            Console.WriteLine($"      Turning Radius: {(item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.TurningRadius))}");

            Console.WriteLine("    External Dimensions:");
            Console.WriteLine($"      Length: {(item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Length))}");
            Console.WriteLine($"      Width: {(item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Width))}");
            Console.WriteLine($"      Height: {(item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Height))}");
            Console.WriteLine($"      Wheelbase: {(item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Wheelbase))}");
            Console.WriteLine($"      Ground Clearance: {(item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.GroundClearance))}");

            Console.WriteLine("    Trunk Dimensions:");
            Console.WriteLine($"      Maximum Trunk Capacity Seats Folded: {(item.BodySpecs.TrunkDimensions is null ? "N/A" : Format(item.BodySpecs.TrunkDimensions.MaximumTrunkCapacitySeatsFolded))}");
            Console.WriteLine($"      Minimum Trunk Capacity Seats Up: {(item.BodySpecs.TrunkDimensions is null ? "N/A" : Format(item.BodySpecs.TrunkDimensions.MinimumTrunkCapacitySeatsUp))}");
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    static void PrintVehicleBodyEngineItems(IEnumerable<VehicleBodyEngine> items)
    {
        foreach (var item in items)
        {
            Console.WriteLine(item.DisplayName);
            Console.WriteLine($"  Engine: {item.Engine?.Name ?? "N/A"}");

            var bodySpecs = item.VehicleBody?.BodySpecs;

            if (bodySpecs is not null)
            {
                Console.WriteLine("  Body Specs:");
                Console.WriteLine("    Basic Parameters:");
                Console.WriteLine($"      Number Of Doors: {(bodySpecs.BasicParameters is null ? "N/A" : Format(bodySpecs.BasicParameters.NumberOfDoors))}");
                Console.WriteLine($"      Number Of Seats: {(bodySpecs.BasicParameters is null ? "N/A" : Format(bodySpecs.BasicParameters.NumberOfSeats))}");
                Console.WriteLine($"      Turning Diameter: {(bodySpecs.BasicParameters is null ? "N/A" : Format(bodySpecs.BasicParameters.TurningDiameter))}");
                Console.WriteLine($"      Turning Radius: {(bodySpecs.BasicParameters is null ? "N/A" : Format(bodySpecs.BasicParameters.TurningRadius))}");
                Console.WriteLine("    External Dimensions:");
                Console.WriteLine($"      Length: {(bodySpecs.ExternalDimensions is null ? "N/A" : Format(bodySpecs.ExternalDimensions.Length))}");
                Console.WriteLine($"      Width: {(bodySpecs.ExternalDimensions is null ? "N/A" : Format(bodySpecs.ExternalDimensions.Width))}");
                Console.WriteLine($"      Height: {(bodySpecs.ExternalDimensions is null ? "N/A" : Format(bodySpecs.ExternalDimensions.Height))}");
                Console.WriteLine($"      Wheelbase: {(bodySpecs.ExternalDimensions is null ? "N/A" : Format(bodySpecs.ExternalDimensions.Wheelbase))}");
                Console.WriteLine($"      Ground Clearance: {(bodySpecs.ExternalDimensions is null ? "N/A" : Format(bodySpecs.ExternalDimensions.GroundClearance))}");
                Console.WriteLine("    Trunk Dimensions:");
                Console.WriteLine($"      Maximum Trunk Capacity Seats Folded: {(bodySpecs.TrunkDimensions is null ? "N/A" : Format(bodySpecs.TrunkDimensions.MaximumTrunkCapacitySeatsFolded))}");
                Console.WriteLine($"      Minimum Trunk Capacity Seats Up: {(bodySpecs.TrunkDimensions is null ? "N/A" : Format(bodySpecs.TrunkDimensions.MinimumTrunkCapacitySeatsUp))}");
            }

            Console.WriteLine("  Engine Specs:");
            Console.WriteLine($"    Capacity: {Format(item.EngineSpecs.Capacity)}");
            Console.WriteLine($"    Fuel Type: {item.EngineSpecs.FuelType}");
            Console.WriteLine("    Architecture:");
            Console.WriteLine($"      Cylinder Count: {(item.EngineSpecs.Architecture is null ? "N/A" : Format(item.EngineSpecs.Architecture.CylinderCount))}");
            Console.WriteLine($"      Cylinder Arrangement: {(item.EngineSpecs.Architecture is null ? "N/A" : item.EngineSpecs.Architecture.CylinderArrangement)}");
            Console.WriteLine($"      Valve Count: {(item.EngineSpecs.Architecture is null ? "N/A" : Format(item.EngineSpecs.Architecture.ValveCount))}");
            Console.WriteLine("    Power:");
            Console.WriteLine($"      Horsepower: {(item.EngineSpecs.Power is null ? "N/A" : Format(item.EngineSpecs.Power.Horsepower))}");
            Console.WriteLine($"      At RPM: {(item.EngineSpecs.Power is null ? "N/A" : Format(item.EngineSpecs.Power.AtRpm))}");
            Console.WriteLine("    Torque:");
            Console.WriteLine($"      Max Torque: {(item.EngineSpecs.Torque is null ? "N/A" : Format(item.EngineSpecs.Torque.MaxTorque))}");
            Console.WriteLine($"      At RPM From: {(item.EngineSpecs.Torque is null ? "N/A" : Format(item.EngineSpecs.Torque.AtRpmFrom))}");
            Console.WriteLine($"      At RPM To: {(item.EngineSpecs.Torque is null ? "N/A" : Format(item.EngineSpecs.Torque.AtRpmTo))}");

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    static void PrintVehicleBodyEngineVariantItems(IEnumerable<VehicleBodyEngineVariant> items)
    {
        foreach (var item in items)
        {
            Console.WriteLine(item.DisplayName);
            Console.WriteLine("  Body Specs:");
            Console.WriteLine("    Basic Parameters:");
            Console.WriteLine($"      Number Of Doors: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters.NumberOfDoors))}");
            Console.WriteLine($"      Number Of Seats: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters.NumberOfSeats))}");
            Console.WriteLine($"      Turning Diameter: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters.TurningDiameter))}");
            Console.WriteLine($"      Turning Radius: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.BasicParameters.TurningRadius))}");
            Console.WriteLine("    External Dimensions:");
            Console.WriteLine($"      Length: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions.Length))}");
            Console.WriteLine($"      Width: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions.Width))}");
            Console.WriteLine($"      Height: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions.Height))}");
            Console.WriteLine($"      Wheelbase: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions.Wheelbase))}");
            Console.WriteLine($"      Ground Clearance: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.ExternalDimensions.GroundClearance))}");
            Console.WriteLine("    Trunk Dimensions:");
            Console.WriteLine($"      Maximum Trunk Capacity Seats Folded: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.TrunkDimensions is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.TrunkDimensions.MaximumTrunkCapacitySeatsFolded))}");
            Console.WriteLine($"      Minimum Trunk Capacity Seats Up: {(item.VehicleBodyEngine.VehicleBody.BodySpecs.TrunkDimensions is null ? "N/A" : Format(item.VehicleBodyEngine.VehicleBody.BodySpecs.TrunkDimensions.MinimumTrunkCapacitySeatsUp))}");

            Console.WriteLine("  Engine Specs:");
            Console.WriteLine($"    Capacity: {Format(item.VehicleBodyEngine.EngineSpecs.Capacity)}");
            Console.WriteLine($"    Fuel Type: {item.VehicleBodyEngine.EngineSpecs.FuelType}");
            Console.WriteLine("    Architecture:");
            Console.WriteLine($"      Cylinder Count: {(item.VehicleBodyEngine.EngineSpecs.Architecture is null ? "N/A" : Format(item.VehicleBodyEngine.EngineSpecs.Architecture.CylinderCount))}");
            Console.WriteLine($"      Cylinder Arrangement: {(item.VehicleBodyEngine.EngineSpecs.Architecture is null ? "N/A" : item.VehicleBodyEngine.EngineSpecs.Architecture.CylinderArrangement)}");
            Console.WriteLine($"      Valve Count: {(item.VehicleBodyEngine.EngineSpecs.Architecture is null ? "N/A" : Format(item.VehicleBodyEngine.EngineSpecs.Architecture.ValveCount))}");
            Console.WriteLine("    Power:");
            Console.WriteLine($"      Horsepower: {(item.VehicleBodyEngine.EngineSpecs.Power is null ? "N/A" : Format(item.VehicleBodyEngine.EngineSpecs.Power.Horsepower))}");
            Console.WriteLine($"      At RPM: {(item.VehicleBodyEngine.EngineSpecs.Power is null ? "N/A" : Format(item.VehicleBodyEngine.EngineSpecs.Power.AtRpm))}");
            Console.WriteLine("    Torque:");
            Console.WriteLine($"      Max Torque: {(item.VehicleBodyEngine.EngineSpecs.Torque is null ? "N/A" : Format(item.VehicleBodyEngine.EngineSpecs.Torque.MaxTorque))}");
            Console.WriteLine($"      At RPM From: {(item.VehicleBodyEngine.EngineSpecs.Torque is null ? "N/A" : Format(item.VehicleBodyEngine.EngineSpecs.Torque.AtRpmFrom))}");
            Console.WriteLine($"      At RPM To: {(item.VehicleBodyEngine.EngineSpecs.Torque is null ? "N/A" : Format(item.VehicleBodyEngine.EngineSpecs.Torque.AtRpmTo))}");

            Console.WriteLine("  Drivetrain Specs:");
            Console.WriteLine($"    Transmission Type: {(item.EngineVariantSpecs.DrivetrainSpecs is null ? "N/A" : item.EngineVariantSpecs.DrivetrainSpecs.TransmissionType.ToString())}");
            Console.WriteLine($"    Drivetrain: {(item.EngineVariantSpecs.DrivetrainSpecs is null ? "N/A" : item.EngineVariantSpecs.DrivetrainSpecs.Drivetrain.ToString())}");

            Console.WriteLine("  Performance Specs:");
            Console.WriteLine($"    Acceleration 0-100: {(item.EngineVariantSpecs.PerformanceSpecs is null ? "N/A" : Format(item.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100))}");
            Console.WriteLine($"    Top Speed: {(item.EngineVariantSpecs.PerformanceSpecs is null ? "N/A" : Format(item.EngineVariantSpecs.PerformanceSpecs.TopSpeed))}");

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    static string Format(ParameterValue? value)
    {
        if (value is null || !value.HasValue)
        {
            return "N/A";
        }

        return value.Unit.HasValue
            ? $"{value.Value} {value.Unit.Value}"
            : value.Value?.ToString() ?? "N/A";
    }
}

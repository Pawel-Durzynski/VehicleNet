using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using VehicleNet.Catalog.Extensions;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Catalog;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;
using VehicleNet.Common.Models.Units;
using VehicleNet.ConsoleTest;
using VehicleNet.Vin.Extensions;

var services = new ServiceCollection();
services.AddVehicleCatalogServices();
services.AddVehicleVinServices();

services.AddTransient<VehicleNetCatalog>();
services.AddTransient<VehicleNetVin>();

using var serviceProvider = services.BuildServiceProvider();
var catalogPlayground = serviceProvider.GetRequiredService<VehicleNetCatalog>();
catalogPlayground.Playground();

var vinPlayground = serviceProvider.GetRequiredService<VehicleNetVin>();
vinPlayground.Playground();

var manufacturerService = serviceProvider.GetRequiredService<IManufacturerService>();
var modelService = serviceProvider.GetRequiredService<IModelService>();
var generationService = serviceProvider.GetRequiredService<IGenerationService>();
var versionService = serviceProvider.GetRequiredService<IVersionService>();
var engineService = serviceProvider.GetRequiredService<IEngineService>();
var engineVariantService = serviceProvider.GetRequiredService<IEngineVariantService>();
var vehicleBodyEngineVariantService = serviceProvider.GetRequiredService<IVehicleBodyEngineVariantService>();

var manufacturers = manufacturerService.Search(new ManufacturerSearch())
    .OrderBy(x => x.Name)
    .DistinctBy(x => x.Id)
    .ToList();

var selectedManufacturer = PromptSingleSelection("Select manufacturer", manufacturers, x => x.Name);
if (selectedManufacturer is null)
{
    return;
}

var models = modelService.Search(new ModelSearch { ManufacturerId = selectedManufacturer.Id })
    .OrderBy(x => x.Name)
    .DistinctBy(x => x.Id)
    .ToList();

var selectedModel = PromptSingleSelection("Select model", models, x => x.Name);
if (selectedModel is null)
{
    return;
}

var generations = generationService.Search(new GenerationSearch { ModelId = selectedModel.Id })
    .OrderBy(x => x.StartYear)
    .ThenBy(x => x.Name)
    .DistinctBy(x => x.Id)
    .ToList();

var selectedGeneration = PromptSingleSelection("Select generation", generations, x => $"{x.Name} ({x.StartYear}-{FormatYear(x.EndYear)})");
if (selectedGeneration is null)
{
    return;
}

VehicleVersion? selectedVersion = null;
if (selectedGeneration.ContainsVersions)
{
    var versions = versionService.Search(new VersionSearch { GenerationId = selectedGeneration.Id })
        .OrderBy(x => x.StartYear)
        .ThenBy(x => x.Name)
        .DistinctBy(x => x.Id)
        .ToList();

    selectedVersion = PromptSingleSelection("Select version", versions, x => x.DisplayName);
    if (selectedVersion is null)
    {
        return;
    }
}

var engines = selectedGeneration.ContainsVersions
    ? engineService.Search(new EngineSearch { VersionId = selectedVersion!.Id })
    : engineService.Search(new EngineSearch { GenerationId = selectedGeneration.Id });

var engineChoices = engines
    .OrderBy(x => x.Name)
    .DistinctBy(x => x.Id)
    .ToList();

var selectedEngine = PromptSingleSelection("Select engine", engineChoices, x => x.Name);
if (selectedEngine is null)
{
    return;
}

var engineVariants = engineVariantService.Search(new EngineVariantSearch { EngineId = selectedEngine.Id })
    .OrderBy(x => x.Name)
    .DistinctBy(x => x.EngineVariantId)
    .ToList();

var selectedEngineVariant = PromptSingleSelection("Select engine version", engineVariants, x => x.Name);
if (selectedEngineVariant is null)
{
    return;
}

var vehicleBodyEngineVariants = vehicleBodyEngineVariantService
    .Search(new VehicleBodyEngineVariantSearch { EngineVariantId = selectedEngineVariant.EngineVariantId })
    .Items
    .DistinctBy(x => x.VehicleBodyEngineVariantId)
    .ToList();

var bodySpecs = vehicleBodyEngineVariants
    .Select(x => x.VehicleBodyEngine.VehicleBody)
    .Where(x => x is not null)
    .Cast<VehicleBody>()
    .DistinctBy(x => x.VehicleBodyId)
    .ToList();

var engineSpecs = vehicleBodyEngineVariants
    .Select(x => x.VehicleBodyEngine)
    .DistinctBy(x => x.VehicleBodyEngineId)
    .ToList();

RenderBodySpecTable(bodySpecs);
RenderEngineSpecTable(engineSpecs);
RenderEngineVersionSpecTable(vehicleBodyEngineVariants);

static T? PromptSingleSelection<T>(string title, IReadOnlyList<T> choices, Func<T, string> display)
    where T : class
{
    if (choices.Count == 0)
    {
        AnsiConsole.MarkupLine($"[red]{title}: no data available.[/]");
        return null;
    }

    var prompt = new SelectionPrompt<T>()
        .Title($"[cyan]{title}[/]")
        .UseConverter(display)
        .AddChoices(choices);

    return AnsiConsole.Prompt(prompt);
}

static void RenderBodySpecTable(IReadOnlyList<VehicleBody> items)
{
    var table = new Table().Title("[yellow]BodySpec[/]").Border(TableBorder.Rounded);
    table.AddColumns("Key", "Value");

    if (items.Count == 0)
    {
        table.AddRow("Info", "No data");
        AnsiConsole.Write(table);
        return;
    }

    foreach (var item in items)
    {
        table.AddRow("Vehicle", item.DisplayName);
        table.AddRow("Doors", Format(item.BodySpecs.BasicParameters.NumberOfDoors));
        table.AddRow("Seats", Format(item.BodySpecs.BasicParameters.NumberOfSeats));
        table.AddRow("Turning diameter", Format(item.BodySpecs.BasicParameters.TurningDiameter));
        table.AddRow("Turning radius", Format(item.BodySpecs.BasicParameters.TurningRadius));
        table.AddRow("Length", Format(item.BodySpecs.ExternalDimensions.Length));
        table.AddRow("Width", Format(item.BodySpecs.ExternalDimensions.Width));
        table.AddRow("Height", Format(item.BodySpecs.ExternalDimensions.Height));
        table.AddRow("Wheelbase", Format(item.BodySpecs.ExternalDimensions.Wheelbase));
        table.AddRow("Ground clearance", Format(item.BodySpecs.ExternalDimensions.GroundClearance));
        table.AddRow("Trunk min", Format(item.BodySpecs.TrunkDimensions.MinimumTrunkCapacitySeatsUp));
        table.AddRow("Trunk max", Format(item.BodySpecs.TrunkDimensions.MaximumTrunkCapacitySeatsFolded));
        table.AddRow("", "");
    }

    AnsiConsole.Write(table);
}

static void RenderEngineSpecTable(IReadOnlyList<VehicleBodyEngine> items)
{
    var table = new Table().Title("[yellow]EngineSpec[/]").Border(TableBorder.Rounded);
    table.AddColumns("Key", "Value");

    if (items.Count == 0)
    {
        table.AddRow("Info", "No data");
        AnsiConsole.Write(table);
        return;
    }

    foreach (var item in items)
    {
        table.AddRow("Vehicle", item.DisplayName);
        table.AddRow("Engine", item.Engine.Name);
        table.AddRow("Capacity", Format(item.EngineSpecs.Capacity));
        table.AddRow("Fuel", item.EngineSpecs.FuelType.ToString());
        table.AddRow("Cylinders", Format(item.EngineSpecs.Architecture.CylinderCount));
        table.AddRow("Arrangement", string.IsNullOrWhiteSpace(item.EngineSpecs.Architecture.CylinderArrangement) ? "N/A" : item.EngineSpecs.Architecture.CylinderArrangement);
        table.AddRow("Valves", Format(item.EngineSpecs.Architecture.ValveCount));
        table.AddRow("Horsepower", Format(item.EngineSpecs.Power.Horsepower));
        table.AddRow("Power rpm", Format(item.EngineSpecs.Power.AtRpm));
        table.AddRow("Torque", Format(item.EngineSpecs.Torque.MaxTorque));
        table.AddRow("Torque rpm", $"{Format(item.EngineSpecs.Torque.AtRpmFrom)} - {Format(item.EngineSpecs.Torque.AtRpmTo)}");
        table.AddRow("", "");
    }

    AnsiConsole.Write(table);
}

static void RenderEngineVersionSpecTable(IReadOnlyList<VehicleBodyEngineVariant> items)
{
    var table = new Table().Title("[yellow]EngineVersionSpec[/]").Border(TableBorder.Rounded);
    table.AddColumns("Key", "Value");

    if (items.Count == 0)
    {
        table.AddRow("Info", "No data");
        AnsiConsole.Write(table);
        return;
    }

    foreach (var item in items)
    {
        table.AddRow("Vehicle", item.DisplayName);
        table.AddRow("Engine version", item.EngineVariant.Name);
        table.AddRow("Transmission", item.EngineVariantSpecs.DrivetrainSpecs.TransmissionType.ToString());
        table.AddRow("Drivetrain", item.EngineVariantSpecs.DrivetrainSpecs.Drivetrain.ToString());
        table.AddRow("0-100", Format(item.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100));
        table.AddRow("Top speed", Format(item.EngineVariantSpecs.PerformanceSpecs.TopSpeed));
        table.AddRow("", "");
    }

    AnsiConsole.Write(table);
}

static string Format(ParameterValue? value)
{
    if (value is null || value.IsMissing || !value.Value.HasValue)
    {
        return "N/A";
    }

    if (value.Unit is null || value.Unit == MeasurementUnit.None)
    {
        return value.Value.Value.ToString("0.##");
    }

    return $"{value.Value.Value:0.##} {value.Unit}";
}

static string FormatYear(int? year) => year?.ToString() ?? "present";

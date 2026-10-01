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

VehicleManufacturer? selectedManufacturer = null;
VehicleModel? selectedModel = null;
VehicleGeneration? selectedGeneration = null;
VehicleVersion? selectedVersion = null;
VehicleEngine? selectedEngine = null;
var stage = CatalogStage.Manufacturer;

while (true)
{
    switch (stage)
    {
        case CatalogStage.Manufacturer:
            selectedManufacturer = PromptSingleSelection("Select manufacturer", manufacturers, x => x.Name, includeBack: false);
            if (selectedManufacturer is null)
            {
                return;
            }

            stage = CatalogStage.Model;
            break;

        case CatalogStage.Model:
            var models = modelService.Search(new ModelSearch { ManufacturerId = selectedManufacturer!.Id })
                .OrderBy(x => x.Name)
                .DistinctBy(x => x.Id)
                .ToList();

            selectedModel = PromptSingleSelection("Select model", models, x => x.Name);
            stage = selectedModel is null ? CatalogStage.Manufacturer : CatalogStage.Generation;
            break;

        case CatalogStage.Generation:
            var generations = generationService.Search(new GenerationSearch { ModelId = selectedModel!.Id })
                .OrderBy(x => x.StartYear)
                .ThenBy(x => x.Name)
                .DistinctBy(x => x.Id)
                .ToList();

            selectedGeneration = PromptSingleSelection("Select generation", generations, x => $"{x.Name} ({x.StartYear}-{FormatYear(x.EndYear)})");
            stage = selectedGeneration is null
                ? CatalogStage.Model
                : selectedGeneration.ContainsVersions ? CatalogStage.Version : CatalogStage.Engine;
            break;

        case CatalogStage.Version:
            var versions = versionService.Search(new VersionSearch { GenerationId = selectedGeneration!.Id })
                .OrderBy(x => x.StartYear)
                .ThenBy(x => x.Name)
                .DistinctBy(x => x.Id)
                .ToList();

            selectedVersion = PromptSingleSelection("Select version", versions, x => x.DisplayName);
            stage = selectedVersion is null ? CatalogStage.Generation : CatalogStage.Engine;
            break;

        case CatalogStage.Engine:
            var engines = selectedGeneration!.ContainsVersions
                ? engineService.Search(new EngineSearch { VersionId = selectedVersion!.Id })
                : engineService.Search(new EngineSearch { GenerationId = selectedGeneration.Id });

            var engineChoices = engines
                .OrderBy(x => x.Name)
                .DistinctBy(x => x.Id)
                .ToList();

            selectedEngine = PromptSingleSelection("Select engine", engineChoices, x => x.Name);
            stage = selectedEngine is null
                ? selectedGeneration.ContainsVersions ? CatalogStage.Version : CatalogStage.Generation
                : CatalogStage.EngineVariant;
            break;

        case CatalogStage.EngineVariant:
            var engineVariants = engineVariantService.Search(new EngineVariantSearch { EngineId = selectedEngine!.Id })
                .OrderBy(x => x.Name)
                .DistinctBy(x => x.EngineVariantId)
                .ToList();

            var selectedEngineVariant = PromptSingleSelection("Select engine version", engineVariants, x => x.Name);
            if (selectedEngineVariant is null)
            {
                stage = CatalogStage.Engine;
                break;
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
            stage = PromptResultsNavigation() == ResultsNavigation.MainMenu
                ? CatalogStage.Manufacturer
                : CatalogStage.EngineVariant;
            break;
    }
}

static T? PromptSingleSelection<T>(string title, IReadOnlyList<T> choices, Func<T, string> display, bool includeBack = true)
    where T : class
{
    if (choices.Count == 0)
    {
        AnsiConsole.MarkupLine($"[red]{title}: no data available.[/]");
        return null;
    }

    var back = new SelectionChoice<T>(null, "[grey]Back[/]");
    var prompt = new SelectionPrompt<SelectionChoice<T>>()
        .Title($"[cyan]{title}[/]")
        .UseConverter(x => x.Label);
    if (includeBack)
    {
        prompt.AddChoice(back);
    }

    prompt.AddChoices(choices.Select(x => new SelectionChoice<T>(x, display(x))));

    var selection = AnsiConsole.Prompt(prompt);
    if (selection.Value is null)
    {
        AnsiConsole.Clear();
    }

    return selection.Value;
}

static ResultsNavigation PromptResultsNavigation()
{
    var prompt = new SelectionPrompt<ResultsNavigation>()
        .UseConverter(x => x == ResultsNavigation.Back ? "Back" : "Main Menu");
    prompt.AddChoices(ResultsNavigation.Back, ResultsNavigation.MainMenu);
    var selection = AnsiConsole.Prompt(prompt);
    AnsiConsole.Clear();
    return selection;
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
        table.AddRow("Doors", item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.NumberOfDoors));
        table.AddRow("Seats", item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.NumberOfSeats));
        table.AddRow("Turning diameter", item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.TurningDiameter));
        table.AddRow("Turning radius", item.BodySpecs.BasicParameters is null ? "N/A" : Format(item.BodySpecs.BasicParameters.TurningRadius));
        table.AddRow("Length", item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Length));
        table.AddRow("Width", item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Width));
        table.AddRow("Height", item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Height));
        table.AddRow("Wheelbase", item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.Wheelbase));
        table.AddRow("Ground clearance", item.BodySpecs.ExternalDimensions is null ? "N/A" : Format(item.BodySpecs.ExternalDimensions.GroundClearance));
        table.AddRow("Trunk min", item.BodySpecs.TrunkDimensions is null ? "N/A" : Format(item.BodySpecs.TrunkDimensions.MinimumTrunkCapacitySeatsUp));
        table.AddRow("Trunk max", item.BodySpecs.TrunkDimensions is null ? "N/A" : Format(item.BodySpecs.TrunkDimensions.MaximumTrunkCapacitySeatsFolded));
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
        table.AddRow("Cylinders", item.EngineSpecs.Architecture is null ? "N/A" : Format(item.EngineSpecs.Architecture.CylinderCount));
        table.AddRow("Arrangement", item.EngineSpecs.Architecture is null || string.IsNullOrWhiteSpace(item.EngineSpecs.Architecture.CylinderArrangement) ? "N/A" : item.EngineSpecs.Architecture.CylinderArrangement);
        table.AddRow("Valves", item.EngineSpecs.Architecture is null ? "N/A" : Format(item.EngineSpecs.Architecture.ValveCount));
        table.AddRow("Horsepower", item.EngineSpecs.Power is null ? "N/A" : Format(item.EngineSpecs.Power.Horsepower));
        table.AddRow("Power rpm", item.EngineSpecs.Power is null ? "N/A" : Format(item.EngineSpecs.Power.AtRpm));
        table.AddRow("Torque", item.EngineSpecs.Torque is null ? "N/A" : Format(item.EngineSpecs.Torque.MaxTorque));
        table.AddRow("Torque rpm", item.EngineSpecs.Torque is null ? "N/A" : $"{Format(item.EngineSpecs.Torque.AtRpmFrom)} - {Format(item.EngineSpecs.Torque.AtRpmTo)}");
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
        table.AddRow("Transmission", item.EngineVariantSpecs.DrivetrainSpecs is null ? "N/A" : item.EngineVariantSpecs.DrivetrainSpecs.TransmissionType.ToString());
        table.AddRow("Drivetrain", item.EngineVariantSpecs.DrivetrainSpecs is null ? "N/A" : item.EngineVariantSpecs.DrivetrainSpecs.Drivetrain.ToString());
        table.AddRow("0-100", item.EngineVariantSpecs.PerformanceSpecs is null ? "N/A" : Format(item.EngineVariantSpecs.PerformanceSpecs.Acceleration0To100));
        table.AddRow("Top speed", item.EngineVariantSpecs.PerformanceSpecs is null ? "N/A" : Format(item.EngineVariantSpecs.PerformanceSpecs.TopSpeed));
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

file sealed record SelectionChoice<T>(T? Value, string Label)
    where T : class;

file enum CatalogStage
{
    Manufacturer,
    Model,
    Generation,
    Version,
    Engine,
    EngineVariant
}

file enum ResultsNavigation
{
    Back,
    MainMenu
}

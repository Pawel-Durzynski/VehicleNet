using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using NUnit.Framework;
using VehicleNet.Catalog.Data.Json;
using VehicleNet.Common.Enums;

namespace VehicleNet.Catalog.Test.Unit.Data.Json;

[TestFixture]
public sealed class CatalogJsonDocumentTests
{
    [Test]
    public async Task Validate_WhenAllCatalogJsonFilesAreLoaded_DoesNotThrow()
    {
        var dataDirectory = Path.GetFullPath(
            Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", "VehicleNet.Catalog", "Data"));

        var document = new CatalogJsonDocument
        {
            Manufacturers = await LoadFileAsync(dataDirectory, "1-manufacturers.json", context => context.IReadOnlyListManufacturerDto),
            Models = await LoadFileAsync(dataDirectory, "2-models.json", context => context.IReadOnlyListModelDto),
            Generations = await LoadFileAsync(dataDirectory, "3-generations.json", context => context.IReadOnlyListGenerationDto),
            Versions = await LoadFileAsync(dataDirectory, "4-versions.json", context => context.IReadOnlyListVersionDto),
            Engines = await LoadFileAsync(dataDirectory, "5-engines.json", context => context.IReadOnlyListEngineDto),
            EngineVariants = await LoadFileAsync(dataDirectory, "6-engine-variants.json", context => context.IReadOnlyListEngineVariantDto),
            VehicleBodies = await LoadFileAsync(dataDirectory, "7-vehicle-bodies.json", context => context.IReadOnlyListVehicleBodyDto),
            VehicleBodyEngines = await LoadFileAsync(dataDirectory, "8-vehicle-body-engines.json", context => context.IReadOnlyListVehicleBodyEngineDto),
            VehicleBodyEngineVariants = await LoadFileAsync(dataDirectory, "9-vehicle-body-engine-variants.json", context => context.IReadOnlyListVehicleBodyEngineVariantDto)
        };

        Assert.DoesNotThrow(() => Validate(document));
    }

    [Test]
    public void Validate_WhenDuplicateManufacturerIdsExist_ThrowsInvalidOperationException()
    {
        var document = new CatalogJsonDocument
        {
            Manufacturers =
            [
                new ManufacturerDto { Id = 1, Name = "Audi", Manufacturer = Manufacturer.Audi },
                new ManufacturerDto { Id = 1, Name = "Audi Duplicate", Manufacturer = Manufacturer.Audi }
            ]
        };

        var ex = Assert.Throws<InvalidOperationException>(() => Validate(document));

        Assert.That(ex!.Message, Does.Contain("Duplicate Manufacturer IDs found"));
    }

    [Test]
    public void Validate_WhenIdsAreUnique_DoesNotThrow()
    {
        var document = new CatalogJsonDocument
        {
            Manufacturers =
            [
                new ManufacturerDto { Id = 1, Name = "Audi", Manufacturer = Manufacturer.Audi },
                new ManufacturerDto { Id = 2, Name = "BMW", Manufacturer = Manufacturer.Bmw }
            ]
        };

        Assert.DoesNotThrow(() => Validate(document));
    }

    private static void Validate(CatalogJsonDocument document)
    {
        ValidateUniqueIds(document.Manufacturers, item => item.Id, "Manufacturer");
        ValidateUniqueIds(document.Models, item => item.Id, "Model");
        ValidateUniqueIds(document.Generations, item => item.Id, "Generation");
        ValidateUniqueIds(document.Versions, item => item.Id, "Version");
        ValidateUniqueIds(document.Engines, item => item.Id, "Engine");
        ValidateUniqueIds(document.EngineVariants, item => item.Id, "EngineVariant");
        ValidateUniqueIds(document.VehicleBodies, item => item.Id, "VehicleBody");
        ValidateUniqueIds(document.VehicleBodyEngines, item => item.Id, "VehicleBodyEngine");
        ValidateUniqueIds(document.VehicleBodyEngineVariants, item => item.Id, "VehicleBodyEngineVariant");
    }

    private static void ValidateUniqueIds<T>(IReadOnlyList<T> items, Func<T, int> idSelector, string entityName)
    {
        var duplicates = items
            .GroupBy(idSelector)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicates.Count > 0)
        {
            throw new InvalidOperationException(
                $"Duplicate {entityName} IDs found: {string.Join(", ", duplicates)}. Each {entityName} must have a unique ID.");
        }
    }

    private static async Task<T> LoadFileAsync<T>(
        string dataDirectory,
        string fileName,
        Func<CatalogJsonSerializerContext, JsonTypeInfo<T>> jsonTypeInfoFactory)
    {
        var filePath = Path.Combine(dataDirectory, fileName);
        Assert.That(File.Exists(filePath), Is.True, $"Catalog JSON file was not found: {filePath}");

        await using var stream = File.OpenRead(filePath);
        var result = await JsonSerializer.DeserializeAsync(
            stream,
            jsonTypeInfoFactory(CatalogJsonSerializerContext.Default));

        return result ?? throw new InvalidOperationException($"Catalog JSON file '{filePath}' was empty or invalid.");
    }
}

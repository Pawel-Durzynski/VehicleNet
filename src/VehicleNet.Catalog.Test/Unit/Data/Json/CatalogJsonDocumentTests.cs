using NUnit.Framework;
using VehicleNet.Catalog.Data.Json;
using VehicleNet.Common.Enums;

namespace VehicleNet.Catalog.Test.Unit.Data.Json;

[TestFixture]
public sealed class CatalogJsonDocumentTests
{
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

        var ex = Assert.Throws<InvalidOperationException>(() => document.Validate());

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

        Assert.DoesNotThrow(() => document.Validate());
    }
}

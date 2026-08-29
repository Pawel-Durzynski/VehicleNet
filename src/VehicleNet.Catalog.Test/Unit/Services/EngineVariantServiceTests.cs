using Moq;
using NUnit.Framework;
using VehicleNet.Catalog.Data.Json;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Catalog.Services;
using VehicleNet.Common.Models.Catalog.Hierarchy;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Test.Unit.Services;

[TestFixture]
public sealed class EngineVariantServiceTests
{
    [Test]
    public void Search_FiltersAndSortsVariants_AndResolvesEngineFromDependency()
    {
        var engine = new VehicleEngine(
            Id: 10,
            Name: "2.0 TSI",
            GenerationId: null,
            Generation: null,
            VersionId: null,
            Version: null);

        var engineServiceMock = new Mock<IEngineService>(MockBehavior.Strict);
        engineServiceMock
            .Setup(s => s.Search(It.IsAny<EngineSearch>()))
            .Returns([engine]);

        var variants = new[]
        {
            new EngineVariantDto { EngineVariantId = 2, EngineId = 10, Name = "TSI DSG" },
            new EngineVariantDto { EngineVariantId = 1, EngineId = 10, Name = "TSI Manual" }
        };

        var sut = new EngineVariantService(variants, engineServiceMock.Object);

        var result = sut.Search(new EngineVariantSearch { Name = "TSI" }).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Select(v => v.Name), Is.EqualTo(new[] { "TSI DSG", "TSI Manual" }.OrderBy(name => name)));
            Assert.That(result.All(v => v.Engine.Id == 10), Is.True);
        });

        engineServiceMock.Verify(s => s.Search(It.IsAny<EngineSearch>()), Times.Exactly(2));
    }

    [Test]
    public void Search_WhenEngineIsMissing_ThrowsInvalidOperationException()
    {
        var engineServiceMock = new Mock<IEngineService>(MockBehavior.Strict);
        engineServiceMock
            .Setup(s => s.Search(It.IsAny<EngineSearch>()))
            .Returns(Array.Empty<VehicleEngine>());

        var variants = new[]
        {
            new EngineVariantDto { EngineVariantId = 1, EngineId = 999, Name = "Missing Engine Variant" }
        };

        var sut = new EngineVariantService(variants, engineServiceMock.Object);

        var ex = Assert.Throws<InvalidOperationException>(() => sut.Search(new EngineVariantSearch()).ToList());

        Assert.That(ex!.Message, Does.Contain("Engine 999 was not found for variant 1."));
    }
}

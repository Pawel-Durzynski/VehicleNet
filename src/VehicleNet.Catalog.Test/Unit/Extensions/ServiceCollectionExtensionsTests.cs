using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using VehicleNet.Catalog.Extensions;
using VehicleNet.Catalog.Interfaces;
using VehicleNet.Common.Models.Search;

namespace VehicleNet.Catalog.Test.Unit.Extensions;

[TestFixture]
public sealed class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddVehicleCatalogServices_RegistersAllServices_AndLoadsCatalogData()
    {
        var services = new ServiceCollection();

        services.AddVehicleCatalogServices();
        using var provider = services.BuildServiceProvider();

        Assert.Multiple(() =>
        {
            Assert.That(provider.GetService<IManufacturerService>(), Is.Not.Null);
            Assert.That(provider.GetService<IModelService>(), Is.Not.Null);
            Assert.That(provider.GetService<IGenerationService>(), Is.Not.Null);
            Assert.That(provider.GetService<IVersionService>(), Is.Not.Null);
            Assert.That(provider.GetService<IEngineService>(), Is.Not.Null);
            Assert.That(provider.GetService<IEngineVariantService>(), Is.Not.Null);
            Assert.That(provider.GetService<IVehicleBodyService>(), Is.Not.Null);
            Assert.That(provider.GetService<IVehicleBodyEngineService>(), Is.Not.Null);
            Assert.That(provider.GetService<IVehicleBodyEngineVariantService>(), Is.Not.Null);
        });

        var manufacturerService = provider.GetRequiredService<IManufacturerService>();
        var result = manufacturerService.Search(new ManufacturerSearch()).ToList();

        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Select(m => m.Name), Is.Ordered);
    }
}

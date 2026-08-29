using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using VehicleNet.Vin.Extensions;
using VehicleNet.Vin.Interfaces;

namespace VehicleNet.Vin.Test.Unit.Extensions;

[TestFixture]
public sealed class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddVehicleVinServices_RegistersAllServices_AndCanGenerateValidVin()
    {
        var services = new ServiceCollection();

        services.AddVehicleVinServices();
        using var provider = services.BuildServiceProvider();

        Assert.Multiple(() =>
        {
            Assert.That(provider.GetService<IVinValidator>(), Is.Not.Null);
            Assert.That(provider.GetService<IVinParser>(), Is.Not.Null);
            Assert.That(provider.GetService<IVinGenerator>(), Is.Not.Null);
        });

        var generator = provider.GetRequiredService<IVinGenerator>();
        var validator = provider.GetRequiredService<IVinValidator>();

        var vin = generator.GenerateMockVin(random: new Random(12345));
        var validation = validator.Validate(vin);

        Assert.That(validation.IsValid, Is.True);
    }
}

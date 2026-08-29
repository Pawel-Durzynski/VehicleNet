using NUnit.Framework;
using VehicleNet.Vin.Services;

namespace VehicleNet.Vin.Test.Unit.Services;

[TestFixture]
public sealed class VinValidatorServiceTests
{
    [Test]
    public void Validate_WhenVinIsCorrect_ReturnsSuccess()
    {
        const string vin = "1HGCM82633A004352";
        var sut = new VinValidatorService();

        var result = sut.Validate(vin);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Error, Is.Null);
            Assert.That(sut.IsValid(vin), Is.True);
        });
    }

    [Test]
    public void Validate_WhenCheckDigitIsInvalid_ReturnsFailure()
    {
        const string vin = "1HGCM82634A004352";
        var sut = new VinValidatorService();

        var result = sut.Validate(vin);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Error, Does.Contain("Invalid check digit"));
            Assert.That(sut.IsValid(vin), Is.False);
        });
    }
}

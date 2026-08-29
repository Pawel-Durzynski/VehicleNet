using Moq;
using NUnit.Framework;
using VehicleNet.Vin.Interfaces;
using VehicleNet.Vin.Services;

namespace VehicleNet.Vin.Test.Unit.Services;

[TestFixture]
public sealed class VinParserServiceTests
{
    [Test]
    public void Parse_ValidVin_ReturnsParsedVinParts_AndUsesValidatorDependency()
    {
        const string vin = "1HGCM82633A004352";

        var validatorMock = new Mock<IVinValidator>(MockBehavior.Strict);
        validatorMock
            .Setup(v => v.Validate(vin))
            .Returns(VehicleNet.Common.Models.Vin.VinValidationResult.Success());

        var sut = new VinParserService(validatorMock.Object);

        var result = sut.Parse(vin);

        Assert.Multiple(() =>
        {
            Assert.That(result.Vin, Is.EqualTo(vin));
            Assert.That(result.WorldManufacturerIdentifier, Is.EqualTo("1HG"));
            Assert.That(result.VehicleDescriptorSection, Is.EqualTo("CM8263"));
            Assert.That(result.VehicleIdentifierSection, Is.EqualTo("3A004352"));
            Assert.That(result.CheckDigit, Is.EqualTo('3'));
            Assert.That(result.ModelYearCode, Is.EqualTo('3'));
            Assert.That(result.ModelYear, Is.EqualTo(2003));
            Assert.That(result.PlantCode, Is.EqualTo('A'));
            Assert.That(result.SequentialNumber, Is.EqualTo("004352"));
        });

        validatorMock.Verify(v => v.Validate(vin), Times.Once);
    }

    [Test]
    public void Parse_WhenValidationFails_ThrowsFormatException()
    {
        const string vin = "INVALIDVIN1234567";

        var validatorMock = new Mock<IVinValidator>(MockBehavior.Strict);
        validatorMock
            .Setup(v => v.Validate(vin))
            .Returns(VehicleNet.Common.Models.Vin.VinValidationResult.Failure("Bad VIN"));

        var sut = new VinParserService(validatorMock.Object);

        var ex = Assert.Throws<FormatException>(() => sut.Parse(vin));

        Assert.That(ex!.Message, Is.EqualTo("Bad VIN"));
    }
}

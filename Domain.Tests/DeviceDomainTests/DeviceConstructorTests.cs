using Domain.Models;
using Moq;

namespace Domain.Tests.DeviceDomainTests;

public class ConstructorTests
{

    [Fact]
    public void WhenCreatingDeviceWithId_ThenDeviceIsCreated()
    {
        //arrange
        Guid id = Guid.NewGuid(); ;
        string description = "Work laptop";
        string brand = "Dell";
        string model = "Latitude 14";
        string serialNumber = "1234567890";

        //act & assert
        new Device(id, description, brand, model, serialNumber);

    }

    [Fact]
    public void WhenCreatingDeviceWithValidFields_ThenDeviceIsCreated()
    {
        //arrange
        string description = "Work laptop";
        string brand = "Dell";
        string model = "Latitude 14";
        string serialNumber = "1234567890";

        //act
        Device device = new Device(description, brand, model, serialNumber);

        //assert
        Assert.NotNull(device);
        Assert.Equal(brand, device.Brand);
        Assert.Equal(model, device.Model);
        Assert.Equal(serialNumber, device.SerialNumber);
    }

    [Fact]
    public void WhenCreatingDeviceWithValidFieldsAndId_ThenDeviceIsCreated()
    {
        //arrange
        string description = "Work laptop";
        string brand = "Dell";
        string model = "Latitude 14";
        string serialNumber = "1234567890";

        //act
        Device device = new Device(It.IsAny<Guid>(), description, brand, model, serialNumber);

        //assert
        Assert.NotNull(device);
        Assert.Equal(brand, device.Brand);
        Assert.Equal(model, device.Model);
        Assert.Equal(serialNumber, device.SerialNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Desc")]
    [InlineData("Laptop used for administrative and reporting purposes")]
    public void WhenCreatingDeviceWithInvalidDescription_ThenDeviceIsNotCreated(string description)
    {
        //act & assert
        Assert.Throws<ArgumentException>(() => new Device(description, "Dell", "Latitude 14", "1234567890"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Br")]
    [InlineData("International Devices")]
    public void WhenCreatingDeviceWithInvalidBrand_ThenDeviceIsNotCreated(string brand)
    {
        //act & assert
        Assert.Throws<ArgumentException>(() => new Device("Work laptop", brand, "Latitude 14", "1234567890"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("L")]
    [InlineData("SuperLaptopModelX1000")]
    public void WhenCreatingDeviceWithInvalidModel_ThenDeviceIsNotCreated(string model)
    {
        //act & assert
        Assert.Throws<ArgumentException>(() => new Device("Work laptop", "Dell", model, "1234567890"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("ABC123DEF456GHI789JKL012MNO345PQR678STU901VWX234YZ5")]
    [InlineData("12345678901!234567890")]

    public void WhenCreatingDeviceWithInvalidSerialNumber_ThenDeviceIsNotCreated(string serialNumber)
    {
        //act & assert
        Assert.Throws<ArgumentException>(() => new Device("Work laptop", "Dell", "Latitude 14", serialNumber));
    }

}
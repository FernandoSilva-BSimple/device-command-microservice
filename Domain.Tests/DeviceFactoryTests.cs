using Domain.Factory;
using Domain.IRepository;
using Domain.Visitors;
using Moq;

namespace Domain.Tests;

public class DeviceFactoryTests
{
    [Fact]
    public async Task WhenCreatingDeviceWithValidFields_ThenDeviceIsCreated()
    {
        //arrange
        Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
        deviceRepo.Setup(d => d.Exists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        var deviceFactory = new DeviceFactory(deviceRepo.Object);

        string description = "Work laptop";
        string brand = "Dell";
        string model = "Latitude 14";
        string serialNumber = "1234567890";

        //act
        var result = await deviceFactory.CreateDevice(description, brand, model, serialNumber);

        //assert
        Assert.NotNull(result);
        Assert.Equal(brand, result.Brand);
        Assert.Equal(model, result.Model);
        Assert.Equal(serialNumber, result.SerialNumber);
    }


    [Fact]
    public async Task WhenCreatingDeviceThatAlreadyExists_ThenExceptionIsThrown()
    {
        //arrange
        Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
        deviceRepo.Setup(d => d.Exists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        var deviceFactory = new DeviceFactory(deviceRepo.Object);
        string description = "Work laptop";
        string brand = "Dell";
        string model = "Latitude 14";
        string serialNumber = "1234567890";

        //act & assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await deviceFactory.CreateDevice(description, brand, model, serialNumber));

        Assert.Equal("Device already exists.", exception.Message);
    }

    [Fact]
    public void WhenCreatingDeviceWithValidFieldsAndId_ThenDeviceIsCreated()
    {
        //arrange
        Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
        var deviceFactory = new DeviceFactory(deviceRepo.Object);

        string description = "Work laptop";
        string brand = "Dell";
        string model = "Latitude 14";
        string serialNumber = "1234567890";

        //act
        var result = deviceFactory.CreateDevice(It.IsAny<Guid>(), description, brand, model, serialNumber);

        //assert
        Assert.NotNull(result);
        Assert.Equal(brand, result.Brand);
        Assert.Equal(model, result.Model);
        Assert.Equal(serialNumber, result.SerialNumber);
    }

    [Fact]
    public void WhenCreatingDeviceFromVisitor_ThenDeviceIsCreated()
    {
        //arrange
        Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
        var deviceFactory = new DeviceFactory(deviceRepo.Object);

        Mock<IDeviceVisitor> deviceVisitor = new Mock<IDeviceVisitor>();
        deviceVisitor.Setup(d => d.Id).Returns(Guid.NewGuid());
        deviceVisitor.Setup(d => d.Description).Returns("Work laptop");
        deviceVisitor.Setup(d => d.Brand).Returns("Dell");
        deviceVisitor.Setup(d => d.Model).Returns("Latitude 14");
        deviceVisitor.Setup(d => d.SerialNumber).Returns("1234567890");

        //act
        var result = deviceFactory.CreateDevice(deviceVisitor.Object);

        //assert
        Assert.NotNull(result);
        Assert.Equal("Dell", result.Brand);
        Assert.Equal("Latitude 14", result.Model);
        Assert.Equal("1234567890", result.SerialNumber);
    }
}
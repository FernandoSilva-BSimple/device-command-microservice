using Application.DTO;
using Application.IPublishers;
using Application.Service;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.DeviceServiceTests;

public class AddConsumedDeviceTestsAsync
{

    [Fact]
    public async Task AddConsumedDeviceAsync_ShouldCreateAndAddDevice_WhenDeviceDoesNotExist()
    {
        // Arrange
        var mapperDouble = new Mock<IMapper>();
        var deviceRepoDouble = new Mock<IDeviceRepository>();
        var deviceFactoryDouble = new Mock<IDeviceFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();

        var deviceId = Guid.NewGuid();
        var description = "Work laptop";
        var brand = "Dell";
        var model = "Latitude 14";
        var serialNumber = "1234567890";

        var device = new Mock<IDevice>();
        device.Setup(d => d.Id).Returns(deviceId);
        device.Setup(d => d.Description).Returns(description);
        device.Setup(d => d.Brand).Returns(brand);
        device.Setup(d => d.Model).Returns(model);
        device.Setup(d => d.SerialNumber).Returns(serialNumber);

        var deviceInstance = device.Object;

        var expectedDto = new DeviceDTO(deviceId, description, brand, model, serialNumber);

        deviceRepoDouble
            .Setup(dr => dr.ExistsAsync(deviceId))
            .ReturnsAsync(false);

        deviceFactoryDouble
            .Setup(df => df.CreateDevice(deviceId, description, brand, model, serialNumber))
            .Returns(deviceInstance);

        deviceRepoDouble
            .Setup(dr => dr.AddAsync(deviceInstance))
            .ReturnsAsync(deviceInstance);

        mapperDouble
            .Setup(m => m.Map<DeviceDTO>(deviceInstance))
            .Returns(expectedDto);

        var service = new DeviceService(
            publisherDouble.Object,
            deviceRepoDouble.Object,
            deviceFactoryDouble.Object,
            mapperDouble.Object
        );

        // Act
        var result = await service.AddConsumedDeviceAsync(deviceId, description, brand, model, serialNumber);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Value.Id);
        Assert.Equal(expectedDto.Description, result.Value.Description);
        Assert.Equal(expectedDto.Brand, result.Value.Brand);
        Assert.Equal(expectedDto.Model, result.Value.Model);
        Assert.Equal(expectedDto.SerialNumber, result.Value.SerialNumber);

        deviceRepoDouble.Verify(dr => dr.ExistsAsync(deviceId), Times.Once);
        deviceFactoryDouble.Verify(df => df.CreateDevice(deviceId, description, brand, model, serialNumber), Times.Once);
        deviceRepoDouble.Verify(dr => dr.AddAsync(deviceInstance), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(deviceInstance), Times.Once);
    }

    [Fact]
    public async Task AddConsumedDeviceAsync_ShouldReturnNull_WhenDeviceAlreadyExists()
    {
        // Arrange
        var mapperDouble = new Mock<IMapper>();
        var deviceRepoDouble = new Mock<IDeviceRepository>();
        var deviceFactoryDouble = new Mock<IDeviceFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();

        var deviceId = Guid.NewGuid();
        var description = "Work laptop";
        var brand = "Dell";
        var model = "Latitude 14";
        var serialNumber = "1234567890";

        deviceRepoDouble
            .Setup(dr => dr.ExistsAsync(deviceId))
            .ReturnsAsync(true);

        var service = new DeviceService(
            publisherDouble.Object,
            deviceRepoDouble.Object,
            deviceFactoryDouble.Object,
            mapperDouble.Object
        );

        // Act
        var result = await service.AddConsumedDeviceAsync(deviceId, description, brand, model, serialNumber);

        // Assert
        Assert.Null(result);
        deviceRepoDouble.Verify(dr => dr.ExistsAsync(deviceId), Times.Once);
        deviceFactoryDouble.Verify(df => df.CreateDevice(deviceId, description, brand, model, serialNumber), Times.Never);
        deviceRepoDouble.Verify(dr => dr.AddAsync(It.IsAny<IDevice>()), Times.Never);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(It.IsAny<IDevice>()), Times.Never);
    }

}
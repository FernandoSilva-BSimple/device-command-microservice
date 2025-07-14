using Application.DTO;
using Application.IPublishers;
using Application.Service;
using AutoMapper;
using Contracts.Messages;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.DeviceServiceTests;

public class CreateDeviceTestsAsync
{
    [Fact]
    public async Task CreateDeviceAsync_ShouldCreateDeviceAndPublishMessage_WhenValid()
    {
        // Arrange
        var deviceRepoDouble = new Mock<IDeviceRepository>();
        var deviceFactoryDouble = new Mock<IDeviceFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var mapperDouble = new Mock<IMapper>();

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

        var expectedMessage = new DeviceCreatedMessage(deviceId, description, brand, model, serialNumber);

        deviceFactoryDouble
            .Setup(df => df.CreateDevice(description, brand, model, serialNumber))
            .ReturnsAsync(deviceInstance);

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

        var createDto = new CreateDeviceDTO(description, brand, model, serialNumber);

        // Act
        var result = await service.CreateDeviceAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        Assert.Equal(expectedDto.Id, result.Value.Id);
        Assert.Equal(expectedDto.Description, result.Value.Description);
        Assert.Equal(expectedDto.Brand, result.Value.Brand);
        Assert.Equal(expectedDto.Model, result.Value.Model);
        Assert.Equal(expectedDto.SerialNumber, result.Value.SerialNumber);

        deviceFactoryDouble.Verify(df => df.CreateDevice(description, brand, model, serialNumber), Times.Once);
        deviceRepoDouble.Verify(dr => dr.AddAsync(deviceInstance), Times.Once);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(deviceInstance), Times.Once);
        publisherDouble.Verify(p => p.PublishDeviceCreatedAsync(It.Is<DeviceCreatedMessage>(msg =>
            msg.Id == expectedMessage.Id &&
            msg.Description == expectedMessage.Description &&
            msg.Brand == expectedMessage.Brand &&
            msg.Model == expectedMessage.Model &&
            msg.SerialNumber == expectedMessage.SerialNumber
        )), Times.Once);
    }

    [Fact]
    public async Task CreateDeviceAsync_ShouldReturnError_WhenDeviceAlreadyExists()
    {
        // Arrange
        var deviceRepoDouble = new Mock<IDeviceRepository>();
        var deviceFactoryDouble = new Mock<IDeviceFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var mapperDouble = new Mock<IMapper>();

        var createDto = new CreateDeviceDTO("Work laptop", "Dell", "Latitude 14", "1234567890");

        var expectedException = new ArgumentException("Simulated error");

        deviceFactoryDouble.Setup(f => f.CreateDevice(createDto.Description, createDto.Brand, createDto.Model, createDto.SerialNumber)).ThrowsAsync(expectedException);

        var service = new DeviceService(publisherDouble.Object, deviceRepoDouble.Object, deviceFactoryDouble.Object, mapperDouble.Object);

        // Act
        var result = await service.CreateDeviceAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Simulated error", result.Error!.Message);
        Assert.Null(result.Value);

        deviceFactoryDouble.Verify(df => df.CreateDevice(createDto.Description, createDto.Brand, createDto.Model, createDto.SerialNumber), Times.Once);
        deviceRepoDouble.Verify(dr => dr.AddAsync(It.IsAny<IDevice>()), Times.Never);
        mapperDouble.Verify(m => m.Map<DeviceDTO>(It.IsAny<IDevice>()), Times.Never);
        publisherDouble.Verify(p => p.PublishDeviceCreatedAsync(It.IsAny<DeviceCreatedMessage>()), Times.Never);

    }
}
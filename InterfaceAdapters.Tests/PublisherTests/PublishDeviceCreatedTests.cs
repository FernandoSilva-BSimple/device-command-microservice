using Contracts.Messages;
using Domain.Interfaces;
using InterfaceAdapters.Publisher;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.PublisherTests;

public class PublishDeviceCreatedTests
{
    [Fact]
    public async Task PublishDeviceCreatedAsync_ShouldPublishEventWithCorrectData()
    {
        //arrange
        var publishEndpointDouble = new Mock<IPublishEndpoint>();

        var publisher = new MassTransitPublisher(publishEndpointDouble.Object);

        var deviceDouble = new Mock<IDevice>();

        deviceDouble.Setup(d => d.Id).Returns(Guid.NewGuid());
        deviceDouble.Setup(d => d.Description).Returns("Work laptop");
        deviceDouble.Setup(d => d.Brand).Returns("Dell");
        deviceDouble.Setup(d => d.Model).Returns("Latitude 14");
        deviceDouble.Setup(d => d.SerialNumber).Returns("1234567890");

        var deviceCreatedMessage = new DeviceCreatedMessage(deviceDouble.Object.Id, deviceDouble.Object.Description, deviceDouble.Object.Brand, deviceDouble.Object.Model, deviceDouble.Object.SerialNumber);

        //act
        await publisher.PublishDeviceCreatedAsync(deviceCreatedMessage);

        //assert
        publishEndpointDouble.Verify(p => p.Publish(deviceCreatedMessage, It.IsAny<CancellationToken>()), Times.Once);
    }
}
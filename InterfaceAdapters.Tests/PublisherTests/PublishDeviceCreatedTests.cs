using Domain.Interfaces;
using Domain.Messages;
using InterfaceAdapters.Publisher;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.PublisherTests;

public class PublishDeviceCreatedTests
{
    [Fact]
    public async Task PublishDeviceCreatedAsync_ShouldPublishEventWithCorrectData()
    {
        // Arrange
        var publishEndpointDouble = new Mock<IPublishEndpoint>();
        var publisher = new MassTransitPublisher(publishEndpointDouble.Object);

        var id = Guid.NewGuid();
        var description = "Work laptop";
        var brand = "Dell";
        var model = "Latitude 14";
        var serialNumber = "1234567890";
        Guid? assignmentTempId = null;

        // Act
        await publisher.PublishDeviceCreatedAsync(id, description, brand, model, serialNumber, assignmentTempId);

        // Assert
        publishEndpointDouble.Verify(p => p.Publish(
            It.Is<DeviceCreatedMessage>(msg =>
                msg.Id == id &&
                msg.Description == description &&
                msg.Brand == brand &&
                msg.Model == model &&
                msg.SerialNumber == serialNumber &&
                msg.CorrelationId == assignmentTempId
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

}
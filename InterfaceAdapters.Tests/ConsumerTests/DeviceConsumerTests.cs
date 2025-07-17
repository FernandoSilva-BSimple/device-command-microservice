using Application.Interfaces;
using Domain.Messages;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class DeviceConsumerTests
{
    [Fact]
    public async Task Consume_ShouldCallDeviceServiceAddDeviceAsync()
    {
        //arrange
        var deviceDouble = new Mock<IDeviceService>();
        var deviceConsumer = new DeviceCreatedConsumer(deviceDouble.Object);

        var message = new DeviceCreatedMessage(Guid.NewGuid(), "Work laptop", "Dell", "Latitude 14", "1234567890", null);

        var context = Mock.Of<ConsumeContext<DeviceCreatedMessage>>(c => c.Message == message);

        //act
        await deviceConsumer.Consume(context);

        //assert
        deviceDouble.Verify(ds => ds.AddConsumedDeviceAsync(message.Id, message.Description, message.Brand, message.Model, message.SerialNumber), Times.Once);
    }
}
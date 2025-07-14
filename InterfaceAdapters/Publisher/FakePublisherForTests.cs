using Application.IPublishers;
using Contracts.Messages;

public class FakePublisherForTests : IMessagePublisher
{
    public Task PublishDeviceCreatedAsync(DeviceCreatedMessage message)
    {
        return Task.CompletedTask;
    }
}

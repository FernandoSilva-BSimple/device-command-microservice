using Application.IPublishers;

public class FakePublisherForTests : IMessagePublisher
{
    public Task PublishDeviceCreatedAsync(Guid id, string description, string brand, string model, string serialNumber, Guid? assignmentTempId)
    {
        return Task.CompletedTask;
    }
}

using Application.IPublishers;
using Contracts.Messages;
using Domain.Interfaces;
using MassTransit;

namespace InterfaceAdapters.Publisher;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishDeviceCreatedAsync(Guid id, string description, string brand, string model, string serialNumber, Guid? assignmentTempId)
    {
        var deviceCreatedMessage = new DeviceCreatedMessage(id, description, brand, model, serialNumber, assignmentTempId);
        await _publishEndpoint.Publish(deviceCreatedMessage);
    }
}
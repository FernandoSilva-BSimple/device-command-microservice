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

    public async Task PublishDeviceCreatedAsync(DeviceCreatedMessage deviceCreatedMessage)
    {
        await _publishEndpoint.Publish(deviceCreatedMessage);
    }
}
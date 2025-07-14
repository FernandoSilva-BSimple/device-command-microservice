using Application.Interfaces;
using Contracts.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class DeviceCreatedConsumer : IConsumer<DeviceCreatedMessage>
{
    private readonly IDeviceService _deviceService;

    public DeviceCreatedConsumer(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public async Task Consume(ConsumeContext<DeviceCreatedMessage> context)
    {
        await _deviceService.AddConsumedDeviceAsync(context.Message.Id, context.Message.Description, context.Message.Brand, context.Message.Model, context.Message.SerialNumber);
    }
}
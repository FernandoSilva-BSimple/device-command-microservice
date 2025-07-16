using Application.DTO;
using Application.Interfaces;
using Contracts.Commands;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InterfaceAdapters.Consumers;

public class AssignmentWithoutDeviceCreatedConsumer : IConsumer<CreateDeviceFromAssignmentCommand>
{
    private readonly IDeviceService _deviceService;

    public AssignmentWithoutDeviceCreatedConsumer(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public async Task Consume(ConsumeContext<CreateDeviceFromAssignmentCommand> context)
    {
        var msg = context.Message;

        Console.WriteLine("Device received the message!!!" + msg.CorrelationId);

        var deviceDTO = new CreateDeviceDTO(msg.DeviceDescription, msg.DeviceBrand, msg.DeviceModel, msg.DeviceSerialNumber);

        await _deviceService.AddDeviceFromSagaAsync(deviceDTO, msg.CorrelationId);
    }
}

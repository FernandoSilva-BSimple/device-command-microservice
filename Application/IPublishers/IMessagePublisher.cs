using Contracts.Messages;
using Domain.Interfaces;

namespace Application.IPublishers;

public interface IMessagePublisher
{
    Task PublishDeviceCreatedAsync(DeviceCreatedMessage deviceCreatedMessage);
}
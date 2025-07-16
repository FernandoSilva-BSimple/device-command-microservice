using Contracts.Messages;
using Domain.Interfaces;

namespace Application.IPublishers;

public interface IMessagePublisher
{
    Task PublishDeviceCreatedAsync(Guid id, string description, string brand, string model, string serialNumber, Guid? assignmentTempId);
}
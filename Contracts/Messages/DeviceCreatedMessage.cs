namespace Contracts.Messages;

public record DeviceCreatedMessage(Guid Id, string Description, string Brand, string Model, string SerialNumber, Guid? CorrelationId);
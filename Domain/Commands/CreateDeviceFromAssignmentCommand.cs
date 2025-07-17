namespace Domain.Commands;

public record CreateDeviceFromAssignmentCommand(Guid CorrelationId, Guid CollaboratorId, DateOnly StartDate, DateOnly EndDate, string DeviceDescription, string DeviceBrand, string DeviceModel, string DeviceSerialNumber);

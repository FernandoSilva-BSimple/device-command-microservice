namespace Domain.Visitors;

public interface IDeviceVisitor
{
    public Guid Id { get; }
    public string Description { get; }
    public string Brand { get; }
    public string Model { get; }
    public string SerialNumber { get; }
}
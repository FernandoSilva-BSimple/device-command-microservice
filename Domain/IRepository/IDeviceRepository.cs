namespace Domain.IRepository;

public interface IDeviceRepository
{
    Task<bool> Exists(string brand, string model, string serialNumber);
}
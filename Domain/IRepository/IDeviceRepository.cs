using Domain.Interfaces;

namespace Domain.IRepository;

public interface IDeviceRepository
{
    Task<bool> ExistsAsync(string brand, string model, string serialNumber);
    Task<bool> ExistsAsync(Guid id);
    Task<IDevice?> GetByIdAsync(Guid id);
    Task<IDevice> AddAsync(IDevice device);

}
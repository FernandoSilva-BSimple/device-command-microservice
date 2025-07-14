using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory;

public class DeviceFactory : IDeviceFactory
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceFactory(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<IDevice> CreateDevice(string description, string brand, string model, string serialNumber)
    {
        if (await _deviceRepository.ExistsAsync(brand, model, serialNumber)) throw new ArgumentException("Device already exists.");
        return new Device(description, brand, model, serialNumber);
    }

    public IDevice CreateDevice(Guid id, string description, string brand, string model, string serialNumber)
    {
        return new Device(id, description, brand, model, serialNumber);
    }

    public IDevice CreateDevice(IDeviceVisitor deviceVisitor)
    {
        return new Device(deviceVisitor.Id, deviceVisitor.Description, deviceVisitor.Brand, deviceVisitor.Model, deviceVisitor.SerialNumber);
    }
}
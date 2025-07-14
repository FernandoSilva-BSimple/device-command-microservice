using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory;

public interface IDeviceFactory
{
    Task<Device> CreateDevice(string description, string brand, string model, string serialNumber);
    Device CreateDevice(Guid id, string description, string brand, string model, string serialNumber);
    Device CreateDevice(IDeviceVisitor deviceVisitor);
}
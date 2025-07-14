using Domain.Interfaces;
using Domain.Visitors;

namespace Infrastructure.DataModel;

public class DeviceDataModel : IDeviceVisitor
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public string SerialNumber { get; set; }

    public DeviceDataModel(IDevice device)
    {
        Id = device.Id;
        Description = device.Description;
        Brand = device.Brand;
        Model = device.Model;
        SerialNumber = device.SerialNumber;
    }

    public DeviceDataModel() { }
}
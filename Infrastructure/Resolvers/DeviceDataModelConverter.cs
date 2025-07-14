using AutoMapper;
using Domain.Factory;
using Domain.Models;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class DeviceDataModelConverter : ITypeConverter<DeviceDataModel, Device>
{
    private readonly IDeviceFactory _factory;

    public DeviceDataModelConverter(IDeviceFactory factory)
    {
        _factory = factory;
    }

    public Device Convert(DeviceDataModel source, Device destination, ResolutionContext context)
    {
        return _factory.CreateDevice(source);
    }
}
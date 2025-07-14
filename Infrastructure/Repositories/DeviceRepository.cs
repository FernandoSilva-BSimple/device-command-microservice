using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly IMapper _mapper;
    private readonly DeviceContext _context;
    public DeviceRepository(DeviceContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<bool> ExistsAsync(string brand, string model, string serialNumber)
    {
        var device = await _context.Set<DeviceDataModel>().FirstOrDefaultAsync(d => d.Brand == brand && d.Model == model && d.SerialNumber == serialNumber);
        return device == null ? false : true;
    }

    public async Task<IDevice?> GetByIdAsync(Guid id)
    {
        var deviceDM = await _context.Set<DeviceDataModel>().FirstOrDefaultAsync(d => d.Id == id);
        if (deviceDM == null) return null;

        var device = _mapper.Map<DeviceDataModel, Device>(deviceDM);
        return device;
    }

    public async Task<IDevice> AddAsync(IDevice device)
    {
        var deviceEntity = (Device)device;
        var dataModel = _mapper.Map<Device, DeviceDataModel>(deviceEntity);
        _context.Set<DeviceDataModel>().Add(dataModel);
        await _context.SaveChangesAsync();
        var deviceAdded = _mapper.Map<DeviceDataModel, Device>(dataModel);
        return deviceAdded;
    }
}
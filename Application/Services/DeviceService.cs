using Application.DTO;
using Application.Interfaces;
using Application.IPublishers;
using Domain.Factory;
using Domain.IRepository;
using AutoMapper;
using Domain.Models;
using Domain.Interfaces;
using Contracts.Messages;

namespace Application.Service;

public class DeviceService : IDeviceService
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceFactory _deviceFactory;
    private readonly IMapper _mapper;

    public DeviceService(IMessagePublisher messagePublisher, IDeviceRepository deviceRepository, IDeviceFactory deviceFactory, IMapper mapper)
    {
        _messagePublisher = messagePublisher;
        _deviceRepository = deviceRepository;
        _deviceFactory = deviceFactory;
        _mapper = mapper;
    }

    public async Task<Result<DeviceDTO>> CreateDeviceAsync(CreateDeviceDTO createDeviceDTO)
    {
        try
        {
            var newDevice = await _deviceFactory.CreateDevice(createDeviceDTO.Description, createDeviceDTO.Brand, createDeviceDTO.Model, createDeviceDTO.SerialNumber);
            var addedDevice = await _deviceRepository.AddAsync(newDevice);

            var result = _mapper.Map<DeviceDTO>(addedDevice);

            await _messagePublisher.PublishDeviceCreatedAsync(result.Id, createDeviceDTO.Description, createDeviceDTO.Brand, createDeviceDTO.Model, createDeviceDTO.SerialNumber, null);
            return Result<DeviceDTO>.Success(result);
        }
        catch (ArgumentException ex)
        {
            return Result<DeviceDTO>.Failure(Error.BadRequest(ex.Message));
        }
        catch (Exception ex)
        {
            return Result<DeviceDTO>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task<Result<DeviceDTO>?> AddConsumedDeviceAsync(Guid id, string description, string brand, string model, string serialNumber)
    {
        var existingDevice = await _deviceRepository.ExistsAsync(id);

        if (existingDevice) return null;

        var newDevice = _deviceFactory.CreateDevice(id, description, brand, model, serialNumber);
        var addedDevice = await _deviceRepository.AddAsync(newDevice);

        var dto = _mapper.Map<DeviceDTO>(addedDevice);
        return Result<DeviceDTO>.Success(dto);
    }

    public async Task<DeviceDTO> AddDeviceFromSagaAsync(CreateDeviceDTO createDeviceDTO, Guid assignmentTempId)
    {
        var device = await _deviceFactory.CreateDevice(createDeviceDTO.Description, createDeviceDTO.Brand, createDeviceDTO.Model, createDeviceDTO.SerialNumber);
        await _deviceRepository.AddAsync(device);

        await _messagePublisher.PublishDeviceCreatedAsync(device.Id, device.Description, device.Brand, device.Model, device.SerialNumber, assignmentTempId);
        return _mapper.Map<DeviceDTO>(device);
    }
}
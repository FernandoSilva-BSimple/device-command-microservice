using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests.DeviceRepositoryTests;

public class DeviceExistsAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenAddingDevice_ThenDeviceIsAdded()
    {
        // arrange
        var device = new Device(Guid.NewGuid(), "description1", "brand1", "model1", "serialNumber1");

        _mapper.Setup(m => m.Map<Device, DeviceDataModel>(It.IsAny<Device>()))
               .Returns(new DeviceDataModel(device));

        _mapper.Setup(m => m.Map<DeviceDataModel, Device>(It.IsAny<DeviceDataModel>()))
               .Returns(device);

        var repository = new DeviceRepository(context, _mapper.Object);

        // act
        var result = await repository.AddAsync(device);

        // assert
        var deviceAdded = await context.Devices.FirstOrDefaultAsync(d => d.Id == device.Id);
        Assert.NotNull(deviceAdded);
        Assert.Equal(device.Id, result.Id);
    }

}
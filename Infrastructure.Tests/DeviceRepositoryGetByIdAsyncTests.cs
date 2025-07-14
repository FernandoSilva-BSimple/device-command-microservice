using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests;

public class DeviceRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingById_ThenReturnsDevice()
    {
        //arrange
        var device1 = new Mock<IDevice>();
        var guid1 = Guid.NewGuid();
        var description1 = "description1";
        var brand1 = "brand1";
        var model1 = "model1";
        var serialNumber1 = "serialNumber1";
        device1.Setup(d => d.Id).Returns(guid1);
        device1.Setup(d => d.Description).Returns(description1);
        device1.Setup(d => d.Brand).Returns(brand1);
        device1.Setup(d => d.Model).Returns(model1);
        device1.Setup(d => d.SerialNumber).Returns(serialNumber1);
        var deviceDM1 = new DeviceDataModel(device1.Object);
        context.Devices.Add(deviceDM1);

        var device2 = new Mock<IDevice>();
        var guid2 = Guid.NewGuid();
        var description2 = "description2";
        var brand2 = "brand2";
        var model2 = "model2";
        var serialNumber2 = "serialNumber2"; ;
        device2.Setup(d => d.Id).Returns(guid2);
        device2.Setup(d => d.Description).Returns(description2);
        device2.Setup(d => d.Brand).Returns(brand2);
        device2.Setup(d => d.Model).Returns(model2);
        device2.Setup(d => d.SerialNumber).Returns(serialNumber2);
        var deviceDM2 = new DeviceDataModel(device2.Object);
        context.Devices.Add(deviceDM2);

        await context.SaveChangesAsync();

        var expected = new Mock<IDevice>().Object;

        _mapper.Setup(m => m.Map<DeviceDataModel, Device>(
            It.Is<DeviceDataModel>(t =>
            t.Id == deviceDM1.Id)
        )).Returns(new Device(deviceDM1.Id, deviceDM1.Description, deviceDM1.Brand, deviceDM1.Model, deviceDM1.SerialNumber));

        var repository = new DeviceRepository(context, _mapper.Object);

        //act
        var result = await repository.GetByIdAsync(guid1);

        //asert
        Assert.NotNull(result);
        Assert.Equal(deviceDM1.Id, result.Id);

    }

    [Fact]
    public async Task WhenSearchingByNonExistentId_ThenReturnsNull()
    {
        //arrange
        var device1 = new Mock<IDevice>();
        var guid1 = Guid.NewGuid();
        var description1 = "description1";
        var brand1 = "brand1";
        var model1 = "model1";
        var serialNumber1 = "serialNumber1";
        device1.Setup(d => d.Id).Returns(guid1);
        device1.Setup(d => d.Description).Returns(description1);
        device1.Setup(d => d.Brand).Returns(brand1);
        device1.Setup(d => d.Model).Returns(model1);
        device1.Setup(d => d.SerialNumber).Returns(serialNumber1);
        var deviceDM1 = new DeviceDataModel(device1.Object);
        context.Devices.Add(deviceDM1);

        await context.SaveChangesAsync();

        var guid2 = Guid.NewGuid();

        _mapper.Setup(m => m.Map<DeviceDataModel, Device>(
            It.Is<DeviceDataModel>(t =>
            t.Id == deviceDM1.Id)
        )).Returns(new Device(deviceDM1.Id, deviceDM1.Description, deviceDM1.Brand, deviceDM1.Model, deviceDM1.SerialNumber));

        var repository = new DeviceRepository(context, _mapper.Object);

        //act
        var result = await repository.GetByIdAsync(guid2);

        //asert
        Assert.Null(result);

    }
}
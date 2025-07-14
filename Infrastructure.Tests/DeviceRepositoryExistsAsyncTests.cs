using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests;

public class DeviceRepositoryExistsAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenDeviceExists_ThenExistsAsyncReturnsTrue()
    {
        //arrange
        var device1 = new Mock<IDevice>();
        var guid1 = Guid.NewGuid();
        device1.Setup(d => d.Id).Returns(guid1);
        device1.Setup(d => d.Description).Returns("description1");
        device1.Setup(d => d.Brand).Returns("brand1");
        device1.Setup(d => d.Model).Returns("model1");
        device1.Setup(d => d.SerialNumber).Returns("serialNumber1");
        var deviceDM1 = new DeviceDataModel(device1.Object);
        context.Devices.Add(deviceDM1);

        await context.SaveChangesAsync();

        var repository = new DeviceRepository(context, _mapper.Object);

        //act
        var result = await repository.ExistsAsync("brand1", "model1", "serialNumber1");

        //assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("brand2", "model2", "serialNumber2")]
    [InlineData("brand1", "model2", "serialNumber2")]
    [InlineData("brand1", "model1", "serialNumber2")]
    public async Task WhenDeviceDoesNotExist_ThenExistsAsyncReturnsFalse(string brand, string model, string serialNumber)
    {
        //arrange
        var device1 = new Mock<IDevice>();
        var guid1 = Guid.NewGuid();
        device1.Setup(d => d.Id).Returns(guid1);
        device1.Setup(d => d.Description).Returns("description1");
        device1.Setup(d => d.Brand).Returns("brand1");
        device1.Setup(d => d.Model).Returns("model1");
        device1.Setup(d => d.SerialNumber).Returns("serialNumber1");
        var deviceDM1 = new DeviceDataModel(device1.Object);
        context.Devices.Add(deviceDM1);

        await context.SaveChangesAsync();

        var repository = new DeviceRepository(context, _mapper.Object);

        //act
        var result = await repository.ExistsAsync(brand, model, serialNumber);

        //assert
        Assert.False(result);
    }
}
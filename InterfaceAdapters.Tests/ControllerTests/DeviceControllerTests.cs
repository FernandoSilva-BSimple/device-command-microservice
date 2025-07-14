using System.Net;
using Application.DTO;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebApi.IntegrationTests;
using WebApi.IntegrationTests.Tests;

namespace InterfaceAdapters.Tests.ControllerTests;

public class DeviceControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    public DeviceControllerTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient()) { }

    [Fact]
    public async Task CreateDevice_Returns201Created()
    {
        //arrange
        var createDeviceDTO = new CreateDeviceDTO("Work laptop", "Dell", "Latitude 14", "1234567890");

        //act
        var deviceDTO = await PostAndDeserializeAsync<DeviceDTO>("/api/devices", createDeviceDTO);

        //assert
        Assert.NotNull(deviceDTO);
        Assert.Equal(createDeviceDTO.Description, deviceDTO.Description);
        Assert.Equal(createDeviceDTO.Brand, deviceDTO.Brand);
        Assert.Equal(createDeviceDTO.Model, deviceDTO.Model);
        Assert.Equal(createDeviceDTO.SerialNumber, deviceDTO.SerialNumber);
    }

    [Theory]
    [InlineData("", "Description is required.")]
    [InlineData("Device", "Description must have between 10 and 50 characters.")]
    [InlineData("Laptop used for administrative and technical tasks.", "Description must have between 10 and 50 characters.")]
    public async Task CreateDevice_WithEmptyDescription_Returns400BadRequest(string description, string expectedMessage)
    {
        //arrange
        var createDeviceDTO = new CreateDeviceDTO(description, "Dell", "Latitude 14", "1234567890");

        //act
        var response = await PostAsync("/api/devices", createDeviceDTO);

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedMessage, body);
    }

    [Theory]
    [InlineData("", "Brand is required.")]
    [InlineData("De", "Brand must have between 3 and 20 characters.")]
    [InlineData("International Business Machines", "Brand must have between 3 and 20 characters.")]
    [InlineData("Dell!", "Brand must contain only alphanumeric characters.")]
    public async Task CreateDevice_WithEmptyBrand_Returns400BadRequest(string brand, string expectedMessage)
    {
        //arrange
        var createDeviceDTO = new CreateDeviceDTO("Work laptop", brand, "Latitude 14", "1234567890");

        //act
        var response = await PostAsync("/api/devices", createDeviceDTO);

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedMessage, body);
    }

    [Theory]
    [InlineData("", "Model is required.")]
    [InlineData("L", "Model must have between 2 and 20 characters.")]
    [InlineData("SuperLaptopModelX1000", "Model must have between 2 and 20 characters.")]
    [InlineData("Laptop!", "Model must contain only alphanumeric characters.")]
    public async Task CreateDevice_WithEmptyModel_Returns400BadRequest(string model, string expectedMessage)
    {
        //arrange
        var createDeviceDTO = new CreateDeviceDTO("Work laptop", "Dell", model, "1234567890");

        //act
        var response = await PostAsync("/api/devices", createDeviceDTO);

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedMessage, body);
    }

    [Theory]
    [InlineData("", "Serial number is required.")]
    [InlineData("123", "Serial number must have between 5 and 50 characters.")]
    [InlineData("ABC123DEF456GHI789JKL012MNO345PQR678STU901VWX234YZ5", "Serial number must have between 5 and 50 characters.")]
    [InlineData("12345678901!234567890", "Serial number must contain only letters and numbers.")]
    public async Task CreateDevice_WithEmptySerialNumber_Returns400BadRequest(string serialNumber, string expectedMessage)
    {
        //arrange
        var createDeviceDTO = new CreateDeviceDTO("Work laptop", "Dell", "Latitude 14", serialNumber);

        //act
        var response = await PostAsync("/api/devices", createDeviceDTO);

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedMessage, body);
    }
}
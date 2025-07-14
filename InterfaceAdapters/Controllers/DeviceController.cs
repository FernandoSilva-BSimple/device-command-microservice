using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi;

namespace InterfaceAdapters.Controllers;

[Route("api/devices")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    public async Task<ActionResult<DeviceDTO>> Create([FromBody] CreateDeviceDTO createDeviceDTO)
    {
        var deviceCreated = await _deviceService.CreateDeviceAsync(createDeviceDTO);
        return deviceCreated.ToActionResult();
    }
}
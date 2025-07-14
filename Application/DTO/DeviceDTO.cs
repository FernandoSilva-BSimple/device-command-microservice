namespace Application.DTO;

public class DeviceDTO
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }

    public DeviceDTO(Guid guid, string description, string brand, string model, string serialNumber)
    {
        Id = guid;
        Description = description;
        Brand = brand;
        Model = model;
        SerialNumber = serialNumber;
    }
}
namespace Application.DTO;

public class CreateDeviceDTO
{
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }

    public CreateDeviceDTO(string description, string brand, string model, string serialNumber)
    {
        Description = description;
        Brand = brand;
        Model = model;
        SerialNumber = serialNumber;
    }
}
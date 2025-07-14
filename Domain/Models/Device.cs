using Domain.Interfaces;

namespace Domain.Models;

public class Device : IDevice
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }


    public Device(string description, string brand, string model, string serialNumber)
    {
        ValidateDescription(description);
        ValidateBrand(brand);
        ValidateModel(model);
        ValidateSerialNumber(serialNumber);

        Id = Guid.NewGuid();
        Description = description;
        Brand = brand;
        Model = model;
        SerialNumber = serialNumber;
    }

    public Device(Guid id, string description, string brand, string model, string serialNumber)
    {
        Id = id;
        Description = description;
        Brand = brand;
        Model = model;
        SerialNumber = serialNumber;
    }

    public Device() { }

    private void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (description.Length < 10 || description.Length > 50)
            throw new ArgumentException("Description must have between 10 and 50 characters.", nameof(description));
    }

    private void ValidateBrand(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand is required.", nameof(brand));

        if (brand.Length < 3 || brand.Length > 20)
            throw new ArgumentException("Brand must have between 3 and 20 characters.", nameof(brand));

        if (!brand.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("Brand must contain only alphanumeric characters.", nameof(brand));

    }

    private void ValidateModel(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.", nameof(model));

        if (model.Length < 2 || model.Length > 20)
            throw new ArgumentException("Model must have between 2 and 20 characters.", nameof(model));

        if (!model.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("Model must contain only alphanumeric characters.", nameof(model));

    }

    private void ValidateSerialNumber(string serialNumber)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
            throw new ArgumentException("Serial number is required.", nameof(serialNumber));

        if (serialNumber.Length < 5 || serialNumber.Length > 50)
            throw new ArgumentException("Serial number must have between 5 and 50 characters.", nameof(serialNumber));

        if (!serialNumber.All(char.IsLetterOrDigit))
            throw new ArgumentException("Serial number must contain only letters and numbers.", nameof(serialNumber));
    }
}
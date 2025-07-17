using Application.DTO;
using Application.Interfaces;
using Application.IPublishers;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using InterfaceAdapters;
using InterfaceAdapters.Consumers;
using InterfaceAdapters.Publisher;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DeviceContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<IDeviceService, DeviceService>();

// Publisher and fake publisher
if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddScoped<IMessagePublisher, FakePublisherForTests>();
}
else
{
    builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();
}

//Repositories
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();

//Factories
builder.Services.AddScoped<IDeviceFactory, DeviceFactory>();

//Mappers
builder.Services.AddTransient<DeviceDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<Device, DeviceDTO>();
    cfg.CreateMap<DeviceDTO, Device>();
});


// MassTransit
var instanceId = InstanceInfo.InstanceId;

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DeviceCreatedConsumer>();
    x.AddConsumer<AssignmentWithoutDeviceCreatedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", 5674, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint($"devices-cmd-{instanceId}", e =>
        {
            e.ConfigureConsumer<DeviceCreatedConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("devices-cmd-saga", e =>
        {
            e.ConfigureConsumer<AssignmentWithoutDeviceCreatedConsumer>(ctx);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

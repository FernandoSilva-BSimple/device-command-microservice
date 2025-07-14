using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests;

public class RepositoryTestBase
{
    protected readonly Mock<IMapper> _mapper;
    protected readonly DeviceContext context;

    protected RepositoryTestBase()
    {
        _mapper = new Mock<IMapper>();

        // Configure in-memory database
        var options = new DbContextOptionsBuilder<DeviceContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;

        context = new DeviceContext(options);
    }
}

using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DeviceContext : DbContext
    {
        public virtual DbSet<DeviceDataModel> Devices { get; set; }

        public DeviceContext(DbContextOptions<DeviceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
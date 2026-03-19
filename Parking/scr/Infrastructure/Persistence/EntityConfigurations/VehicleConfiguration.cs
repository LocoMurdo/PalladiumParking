using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.API.scr.Shared.Entities;

namespace Parking.API.scr.Infrastructure.Persistence.EntityConfigurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasIndex(plate => plate.PlateId)
                   .IsUnique();
        }
    }
}

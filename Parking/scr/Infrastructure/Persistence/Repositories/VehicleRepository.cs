using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;

namespace Parking.API.scr.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository(DbContext context) : IVehicleRepository
    {
        public  async Task<bool> VehicleExistAsync(string plateId)
        {
            
        
            var vehicle = await context.Set<Vehicle>()
           .Where(vehicle => vehicle.PlateId == plateId) 
           .Select(vehicle => new { vehicle.Id })
           .AsNoTracking()
           .FirstOrDefaultAsync();

            return vehicle is not null;

        
         }
    }
}

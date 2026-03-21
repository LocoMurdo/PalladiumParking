using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.VehicleUseCase.UseCases
{
    public class DeleteVehicleUseCase(DbContext context)
    {
        public async Task<Result> ExecuteAsync(int id)
        {
            var vehicle = await context.Set<Vehicle>().FindAsync(id);
            if (vehicle is null)
                return Result.NotFound("Vehicle not found.");

            context.Set<Vehicle>().Remove(vehicle);
            await context.SaveChangesAsync();

            return Result.Success("Vehicle deleted successfully.");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.VehicleUseCase.UseCases
{
    public class GetVehicleByIdUseCase(DbContext context)
    {
        public async Task<Result<VehicleResponse>> ExecuteAsync(int id)
        {
            var vehicle = await context.Set<Vehicle>()
                .Where(v => v.Id == id)
                .Select(v => new VehicleResponse
                {
                    Id = v.Id,
                    PlateId = v.PlateId,
                    CarModel = v.CarModel,
                    CreatedAt = v.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (vehicle is null)
                return Result.NotFound("Vehicle not found.");

            return Result.Success(vehicle);
        }
    }
}

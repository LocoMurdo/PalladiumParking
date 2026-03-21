using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.VehicleUseCase.UseCases
{
    public class VehicleResponse
    {
        public int Id { get; init; }
        public string PlateId { get; init; }
        public string? CarModel { get; init; }
        public DateTime? CreatedAt { get; init; }
    }

    public class GetAllVehiclesUseCase(DbContext context)
    {
        public async Task<Result<IEnumerable<VehicleResponse>>> ExecuteAsync()
        {
            var vehicles = await context.Set<Vehicle>()
                .Select(v => new VehicleResponse
                {
                    Id = v.Id,
                    PlateId = v.PlateId,
                    CarModel = v.CarModel,
                    CreatedAt = v.CreatedAt
                })
                .ToListAsync();

            return Result.Success(vehicles.AsEnumerable());
        }
    }
}

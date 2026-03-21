using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.RateUseCase.UseCase
{
    public class RateResponse
    {
        public int Id { get; init; }
        public VehicleType VehicleType { get; init; }
        public decimal PricePerHour { get; init; }
        public bool IsActive { get; init; }
    }

    public class GetAllRatesUseCase(DbContext context)
    {
        public async Task<Result<IEnumerable<RateResponse>>> ExecuteAsync()
        {
            var rates = await context.Set<Rate>()
                .Select(r => new RateResponse
                {
                    Id = r.Id,
                    VehicleType = r.VehicleType,
                    PricePerHour = r.PricePerHour,
                    IsActive = r.IsActive
                })
                .ToListAsync();

            return Result.Success(rates.AsEnumerable());
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class SubscriptionResponse
    {
        public int Id { get; init; }
        public string LicensePlate { get; init; }
        public string? CarModel { get; init; }
        public string VehicleType { get; init; }
        public string Plan { get; init; }
        public decimal Price { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string Status { get; init; }
    }

    public class GetAllSubscriptionsUseCase(DbContext context)
    {
        public async Task<Result<IEnumerable<SubscriptionResponse>>> ExecuteAsync()
        {
            var subscriptions = await context.Set<Subscriptions>()
                .Include(s => s.Vehicle)
                .Include(s => s.Rate)
                .OrderByDescending(s => s.StartDate)
                .Select(s => new SubscriptionResponse
                {
                    Id = s.Id,
                    LicensePlate = s.Vehicle.PlateId,
                    CarModel = s.Vehicle.CarModel,
                    VehicleType = s.Rate.VehicleType.ToString(),
                    Plan = s.Plan.ToString(),
                    Price = s.Price,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Status = s.Status.ToString()
                })
                .ToListAsync();

            return Result.Success(subscriptions.AsEnumerable());
        }
    }
}

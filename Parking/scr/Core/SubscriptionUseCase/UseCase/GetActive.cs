using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class GetActiveSubscriptionsUseCase(DbContext context, Shared.Interfaces.IDatetimeservice dateTime)
    {
        public async Task<Result<IEnumerable<SubscriptionResponse>>> ExecuteAsync()
        {
            var now = dateTime.Now;
            var subscriptions = await context.Set<Subscriptions>()
                .Include(s => s.Vehicle)
                .Include(s => s.Rate)
                .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate > now)
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

            if (!subscriptions.Any())
                return Result.Failure("No active subscriptions found.");

            return Result.Success(subscriptions.AsEnumerable());
        }
    }
}

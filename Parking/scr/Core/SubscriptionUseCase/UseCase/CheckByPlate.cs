using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class CheckSubscriptionResponse
    {
        public bool HasActiveSubscription { get; init; }
        public int? SubscriptionId { get; init; }
        public string? Plan { get; init; }
        public DateTime? EndDate { get; init; }
    }

    public class CheckSubscriptionByPlateUseCase(DbContext context, Shared.Interfaces.IDatetimeservice dateTime)
    {
        public async Task<Result<CheckSubscriptionResponse>> ExecuteAsync(string plate)
        {
            var now = dateTime.Now;
            var subscription = await context.Set<Subscriptions>()
                .Include(s => s.Vehicle)
                .Where(s => s.Vehicle.PlateId == plate
                    && s.Status == SubscriptionStatus.Active
                    && s.EndDate > now)
                .FirstOrDefaultAsync();

            var response = new CheckSubscriptionResponse
            {
                HasActiveSubscription = subscription is not null,
                SubscriptionId = subscription?.Id,
                Plan = subscription?.Plan.ToString(),
                EndDate = subscription?.EndDate
            };

            return Result.Success(response);
        }
    }
}

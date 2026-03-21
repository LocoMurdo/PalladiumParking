using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class CancelSubscriptionUseCase(DbContext context)
    {
        public async Task<Result> ExecuteAsync(int id)
        {
            var subscription = await context.Set<Subscriptions>().FindAsync(id);

            if (subscription is null)
                return Result.NotFound("Subscription not found.");

            if (subscription.Status != SubscriptionStatus.Active)
                return Result.Conflict("Only active subscriptions can be cancelled.");

            subscription.Status = SubscriptionStatus.Cancelled;
            await context.SaveChangesAsync();

            return Result.Success("Subscription cancelled successfully.");
        }
    }
}

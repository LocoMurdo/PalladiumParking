using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class SubscriptionPriceResponse
    {
        public int Id { get; init; }
        public string VehicleType { get; init; }
        public string Plan { get; init; }
        public decimal Price { get; init; }
    }

    public class GetSubscriptionPricesUseCase(DbContext context)
    {
        public async Task<Result<IEnumerable<SubscriptionPriceResponse>>> ExecuteAsync()
        {
            var prices = await context.Set<SubscriptionPrice>()
                .Select(sp => new SubscriptionPriceResponse
                {
                    Id = sp.Id,
                    VehicleType = sp.VehicleType.ToString(),
                    Plan = sp.Plan.ToString(),
                    Price = sp.Price
                })
                .ToListAsync();

            if (!prices.Any())
                return Result.Failure("No subscription prices found.");

            return Result.Success(prices.AsEnumerable());
        }
    }
}

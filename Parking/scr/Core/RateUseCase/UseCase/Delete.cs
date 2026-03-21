using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.RateUseCase.UseCase
{
    public class DeleteRateUseCase(DbContext context)
    {
        public async Task<Result> ExecuteAsync(int id)
        {
            var rate = await context.Set<Rate>().FindAsync(id);
            if (rate is null)
                return Result.NotFound("Rate not found.");

            rate.IsActive = false;
            await context.SaveChangesAsync();

            return Result.Success("Rate deactivated successfully.");
        }
    }
}

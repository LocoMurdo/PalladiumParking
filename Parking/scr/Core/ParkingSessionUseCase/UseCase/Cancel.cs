using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase.UseCase
{
    public class CancelParkingSessionUseCase(DbContext context)
    {
        public async Task<Result> ExecuteAsync(int id)
        {
            var session = await context.Set<ParkingSession>().FindAsync(id);

            if (session is null)
                return Result.NotFound("Parking session not found.");

            if (session.Status != ParkingSession.ParkingSessionStatus.Open)
                return Result.Conflict("Only open sessions can be cancelled.");

            session.Status = ParkingSession.ParkingSessionStatus.Cancelled;
            await context.SaveChangesAsync();

            return Result.Success("Parking session cancelled successfully.");
        }
    }
}

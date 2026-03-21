using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase.UseCase
{
    public class GetAllParkingSessionsUseCase(DbContext context)
    {
        public async Task<Result<IEnumerable<ParkingSessionDetailResponse>>> ExecuteAsync()
        {
            var sessions = await context.Set<ParkingSession>()
                .OrderByDescending(s => s.EntryTime)
                .Select(s => new ParkingSessionDetailResponse
                {
                    Id = s.Id,
                    VisitorPlate = s.VisitorPlate,
                    RateId = s.RateId,
                    EntryTime = s.EntryTime,
                    ExitTime = s.ExitTime,
                    TotalAmount = s.TotalAmount,
                    Status = s.Status
                })
                .ToListAsync();

            return Result.Success(sessions.AsEnumerable());
        }
    }
}

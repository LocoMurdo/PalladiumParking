using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase.UseCase
{
    public class ParkingSessionDetailResponse
    {
        public int Id { get; init; }
        public string VisitorPlate { get; init; }
        public int RateId { get; init; }
        public DateTime? EntryTime { get; init; }
        public DateTime? ExitTime { get; init; }
        public decimal? TotalAmount { get; init; }
        public ParkingSession.ParkingSessionStatus Status { get; init; }
    }

    public class GetParkingSessionByIdUseCase(DbContext context)
    {
        public async Task<Result<ParkingSessionDetailResponse>> ExecuteAsync(int id)
        {
            var session = await context.Set<ParkingSession>()
                .Where(s => s.Id == id)
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
                .FirstOrDefaultAsync();

            if (session is null)
                return Result.NotFound("Parking session not found.");

            return Result.Success(session);
        }
    }
}

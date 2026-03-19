using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase.UseCase
{
    public class OpenParkingSessionResponse
    {
        public int SessionId { get; init; }
        public string VisitorPlate { get; init; }
        public DateTime EntryTime { get; init; }
        public int RateId { get; init; }
    }
    public class GetOpenParkingSessionsUseCase(IParkingSessionRepository repository)
    {
        public async Task<Result<IEnumerable<OpenParkingSessionResponse>>> ExecuteAsync()
        {
            var sessions = await repository.GetOpenSessionsAsync();

            var response = sessions.Select(s => new OpenParkingSessionResponse
            {
                SessionId = s.Id,
                VisitorPlate = s.VisitorPlate,
                EntryTime = s.EntryTime!.Value,
                RateId = s.RateId
            });
            if (!response.Any())
            {
                return Result.Failure("No open parking sessions found.");
            }

            return Result.Success(response);
        }
    }
}

using Parking.API.scr.Shared.Entities;

namespace Parking.API.scr.Shared.Interfaces.Persistence.Repositories
{
    public interface IParkingSessionRepository
    {

        Task<ParkingSession?> GetByIdAsync(int id);
       
       

        Task AddAsync(ParkingSession session);

        Task UpdateAsync(ParkingSession session); 


        Task<decimal> CalculateTotalAsync(ParkingSession session);

       
        
            Task<IEnumerable<ParkingSession>> GetOpenSessionsAsync();
        
    }
}

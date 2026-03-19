using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;

namespace Parking.API.scr.Infrastructure.Persistence.Repositories
{
    public class ParkingSessionRepository : IParkingSessionRepository
    {
        private readonly DbContext context;

        public ParkingSessionRepository(DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ParkingSession session)
        {
            await context.Set<ParkingSession>().AddAsync(session);
        }

        public async Task<ParkingSession?> GetByIdAsync(int id)
        {
            return await context
                .Set<ParkingSession>()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(ParkingSession session)
        {
            context.Set<ParkingSession>().Update(session);
            await context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateTotalAsync(ParkingSession session)
        {
            var rate = await context
                .Set<Rate>()
                .FirstOrDefaultAsync(r => r.Id == session.RateId);

            if (rate == null)
                throw new Exception("Rate not found for the given session.");

            if (session.EntryTime == null || session.ExitTime == null)
                throw new Exception("Both EntryTime and ExitTime must be set to calculate total.");

            var totalHours = (session.ExitTime.Value - session.EntryTime.Value).TotalHours;

            var hoursRounded = Math.Ceiling(totalHours);

            var totalAmount = (decimal)hoursRounded * rate.PricePerHour;

            return totalAmount;
        }

        public async Task<IEnumerable<ParkingSession>> GetOpenSessionsAsync()
        {
            return await context.Set<ParkingSession>()
           .Where(s => s.Status == ParkingSession.ParkingSessionStatus.Open)
           .ToListAsync();
        }
    }
}

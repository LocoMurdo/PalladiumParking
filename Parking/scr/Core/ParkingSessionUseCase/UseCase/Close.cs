using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase.UseCase
{
    public class CloseParkingSessionRequest
    {
        public int SessionId { get; init; }
        public PaymentMethod Method { get; init; }
    }
    public class CloseParkingSessionResponse
    {
        public int SessionId { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime ExitTime { get; init; }
        public int Minutes { get; init; }
    }

    public class CloseParkingSessionUseCase(DbContext context,
        IParkingSessionRepository repository,
        IDatetimeservice dateTime)
    {

        public async Task<Result<CloseParkingSessionResponse>> ExecuteAsync(CloseParkingSessionRequest request  )
        {
            var session = await repository.GetByIdAsync(request.SessionId);
            
            if (session == null)
                return Result.NotFound("Parking session not found");
            
            if(session.Status != ParkingSession.ParkingSessionStatus.Open)
                return Result.Conflict("Parking session is not open");

            session.ExitTime = dateTime.Now; 

            var total = await repository.CalculateTotalAsync(session);
            var cash = await context.Set<CashRegister>()
               .FirstOrDefaultAsync(x => x.Status == CashStatus.Open);

            if (cash == null)
                return Result.Conflict("No open cash register");
            var payment = new Payment
            {
                ParkingSessionId = session.Id,
                Amount = total,
                PaymentDate = dateTime.Now,
                Method = request.Method,
                CashRegisterId = cash.Id
            };

            context.Set<Payment>().Add(payment);
            await context.SaveChangesAsync();

            session.TotalAmount = total;
            session.Status = ParkingSession.ParkingSessionStatus.Closed;
            await repository.UpdateAsync(session);

            var response = new CloseParkingSessionResponse
            {
                SessionId = session.Id,
                TotalAmount = total,
                ExitTime = session.ExitTime.Value,
                Minutes = (int)(session.ExitTime.Value - session.EntryTime).Value.TotalMinutes
            };

            return Result.Success(response);


        }
    }

}

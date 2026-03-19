using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.CashRegisterUseCase.UseCase
{
    public class CloseCashResponse
    {
        public int CashRegisterId { get; set; }

        public decimal OpeningAmount { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalCard { get; set; }
        public decimal TotalSinpe { get; set; }

        public decimal Total { get; set; }

        public DateTime ClosedAt { get; set; }
    }
    public class CloseCashUseCase(DbContext context, IDatetimeservice dateTime)
    {
        public async Task<Result<CloseCashResponse>> ExecuteAsync()
        {
            // 🔍 buscar caja abierta
            var cash = await context.Set<CashRegister>()
                .FirstOrDefaultAsync(x => x.Status == CashStatus.Open);

            if (cash == null)
                return Result.Conflict("No open cash register");

            var hasOpenSessions = await context.Set<ParkingSession>()
            .AnyAsync(s => s.Status == ParkingSession.ParkingSessionStatus.Open);

            if (hasOpenSessions)
                return Result.Conflict("Cannot close cash register, there are open parking sessions");

            // 📥 traer pagos de esa caja
            var payments = await context.Set<Payment>()
                .Where(p => p.CashRegisterId == cash.Id)
                .ToListAsync();

            // 💰 cálculos
            var totalCash = payments
                .Where(p => p.Method == PaymentMethod.Cash)
                .Sum(p => p.Amount);

            var totalCard = payments
                .Where(p => p.Method == PaymentMethod.Card)
                .Sum(p => p.Amount);

            var totalSinpe = payments
                .Where(p => p.Method == PaymentMethod.Sinpe)
                .Sum(p => p.Amount);

            var total = totalCash + totalCard + totalSinpe;

            // 🔒 cerrar caja
            cash.Status = CashStatus.Closed;
            cash.ClosedAt = dateTime.Now;
            cash.ClosingAmount = cash.OpeningAmount + total;

            await context.SaveChangesAsync();

            // 📤 response
            var response = new CloseCashResponse
            {
                CashRegisterId = cash.Id,
                OpeningAmount = cash.OpeningAmount,
                TotalCash = totalCash,
                TotalCard = totalCard,
                TotalSinpe = totalSinpe,
                Total = total,
                ClosedAt = cash.ClosedAt.Value
            };

            return Result.Success(response);
        }
    }
}

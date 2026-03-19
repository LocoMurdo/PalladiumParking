using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.CashRegisterUseCase.UseCase
{
    public class CashHistoryItemResponse
    {
        public int CashRegisterId { get; set; }

        public decimal OpeningAmount { get; set; }
        public decimal ClosingAmount { get; set; }

        public decimal TotalCash { get; set; }
        public decimal TotalCard { get; set; }
        public decimal TotalSinpe { get; set; }

        public DateTime OpenedAt { get; set; }
        public DateTime ClosedAt { get; set; }
    }
    public class GetCashHistoryUseCase(DbContext context, IDatetimeservice dateTime)
    {
        public async Task<Result<List<CashHistoryItemResponse>>> ExecuteAsync()
        {
            // 🔍 traer cajas cerradas
            var cashRegisters = await context.Set<CashRegister>()
                .Where(c => c.Status == CashStatus.Closed)
                .OrderByDescending(c => c.ClosedAt)
                .ToListAsync();

            var result = new List<CashHistoryItemResponse>();

            foreach (var cash in cashRegisters)
            {
                // 📥 pagos de la caja
                var payments = await context.Set<Payment>()
                    .Where(p => p.CashRegisterId == cash.Id)
                    .ToListAsync();

                var totalCash = payments
                    .Where(p => p.Method == PaymentMethod.Cash)
                    .Sum(p => p.Amount);

                var totalCard = payments
                    .Where(p => p.Method == PaymentMethod.Card)
                    .Sum(p => p.Amount);

                var totalSinpe = payments
                    .Where(p => p.Method == PaymentMethod.Sinpe)
                    .Sum(p => p.Amount);

                result.Add(new CashHistoryItemResponse
                {
                    CashRegisterId = cash.Id,
                    OpeningAmount = cash.OpeningAmount,
                    ClosingAmount = cash.ClosingAmount ?? 0,
                    TotalCash = totalCash,
                    TotalCard = totalCard,
                    TotalSinpe = totalSinpe,
                    OpenedAt = cash.OpenedAt,
                    ClosedAt = cash.ClosedAt ?? dateTime.Now
                });
            }

            return Result.Success(result);
        }
    }
}

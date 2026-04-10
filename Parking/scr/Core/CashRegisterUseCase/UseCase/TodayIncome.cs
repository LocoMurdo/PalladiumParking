using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.CashRegisterUseCase.UseCase
{
    public class TodayIncomeResponse
    {
        public int CashRegisterId { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal ParkingIncome { get; set; }
        public decimal SubscriptionIncome { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalCard { get; set; }
        public decimal TotalSinpe { get; set; }
        public int ParkingSessionCount { get; set; }
        public int SubscriptionCount { get; set; }
        public int ActiveParkingSessions { get; set; }
        public int ActiveSubscriptions { get; set; }
    }

    public class GetTodayIncomeUseCase(DbContext context, IDatetimeservice dateTime)
    {
        public async Task<Result<TodayIncomeResponse>> ExecuteAsync()
        {
            var cash = await context.Set<CashRegister>()
                .FirstOrDefaultAsync(x => x.Status == CashStatus.Open);

            if (cash == null)
                return Result.Success(new TodayIncomeResponse());

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

            var parkingPayments = payments.Where(p => p.ParkingSessionId != null).ToList();
            var subscriptionPayments = payments.Where(p => p.ParkingSessionId == null).ToList();

            var activeParkingSessions = await context.Set<ParkingSession>()
                .CountAsync(s => s.Status == ParkingSession.ParkingSessionStatus.Open);

            var activeSubscriptions = await context.Set<Subscriptions>()
                .CountAsync(s => s.Status == SubscriptionStatus.Active && s.EndDate > dateTime.Now);

            return Result.Success(new TodayIncomeResponse
            {
                CashRegisterId = cash.Id,
                OpeningAmount = cash.OpeningAmount,
                TotalIncome = totalCash + totalCard + totalSinpe,
                ParkingIncome = parkingPayments.Sum(p => p.Amount),
                SubscriptionIncome = subscriptionPayments.Sum(p => p.Amount),
                TotalCash = totalCash,
                TotalCard = totalCard,
                TotalSinpe = totalSinpe,
                ParkingSessionCount = parkingPayments.Count,
                SubscriptionCount = subscriptionPayments.Count,
                ActiveParkingSessions = activeParkingSessions,
                ActiveSubscriptions = activeSubscriptions
            });
        }
    }
}

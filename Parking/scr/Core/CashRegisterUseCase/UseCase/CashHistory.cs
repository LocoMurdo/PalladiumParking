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

        // Totales generales por método de pago
        public decimal TotalCash { get; set; }
        public decimal TotalCard { get; set; }
        public decimal TotalSinpe { get; set; }

        // Desglose parking sessions
        public int ParkingSessionCount { get; set; }
        public decimal ParkingSessionTotal { get; set; }
        public decimal ParkingCash { get; set; }
        public decimal ParkingCard { get; set; }
        public decimal ParkingSinpe { get; set; }

        // Desglose suscripciones
        public int SubscriptionCount { get; set; }
        public decimal SubscriptionTotal { get; set; }
        public decimal SubscriptionCash { get; set; }
        public decimal SubscriptionCard { get; set; }
        public decimal SubscriptionSinpe { get; set; }

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

                // Totales generales
                var totalCash = payments.Where(p => p.Method == PaymentMethod.Cash).Sum(p => p.Amount);
                var totalCard = payments.Where(p => p.Method == PaymentMethod.Card).Sum(p => p.Amount);
                var totalSinpe = payments.Where(p => p.Method == PaymentMethod.Sinpe).Sum(p => p.Amount);

                // Parking sessions
                var parkingPayments = payments.Where(p => p.ParkingSessionId != null).ToList();
                var parkingCash = parkingPayments.Where(p => p.Method == PaymentMethod.Cash).Sum(p => p.Amount);
                var parkingCard = parkingPayments.Where(p => p.Method == PaymentMethod.Card).Sum(p => p.Amount);
                var parkingSinpe = parkingPayments.Where(p => p.Method == PaymentMethod.Sinpe).Sum(p => p.Amount);

                // Suscripciones
                var subscriptionPayments = payments.Where(p => p.ParkingSessionId == null).ToList();
                var subscriptionCash = subscriptionPayments.Where(p => p.Method == PaymentMethod.Cash).Sum(p => p.Amount);
                var subscriptionCard = subscriptionPayments.Where(p => p.Method == PaymentMethod.Card).Sum(p => p.Amount);
                var subscriptionSinpe = subscriptionPayments.Where(p => p.Method == PaymentMethod.Sinpe).Sum(p => p.Amount);

                result.Add(new CashHistoryItemResponse
                {
                    CashRegisterId = cash.Id,
                    OpeningAmount = cash.OpeningAmount,
                    ClosingAmount = cash.ClosingAmount ?? 0,
                    TotalCash = totalCash,
                    TotalCard = totalCard,
                    TotalSinpe = totalSinpe,
                    ParkingSessionCount = parkingPayments.Count,
                    ParkingSessionTotal = parkingPayments.Sum(p => p.Amount),
                    ParkingCash = parkingCash,
                    ParkingCard = parkingCard,
                    ParkingSinpe = parkingSinpe,
                    SubscriptionCount = subscriptionPayments.Count,
                    SubscriptionTotal = subscriptionPayments.Sum(p => p.Amount),
                    SubscriptionCash = subscriptionCash,
                    SubscriptionCard = subscriptionCard,
                    SubscriptionSinpe = subscriptionSinpe,
                    OpenedAt = cash.OpenedAt,
                    ClosedAt = cash.ClosedAt ?? dateTime.Now
                });
            }

            return Result.Success(result);
        }
    }
}

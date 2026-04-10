using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class CloseSubscriptionRequest
    {
        public PaymentMethod Method { get; set; }
    }

    public class CloseSubscriptionResponse
    {
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public SubscriptionPlan Plan { get; set; }
        public PaymentMethod Method { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class CloseSubscriptionUseCase(DbContext context, IDatetimeservice dateTime)
    {
        public async Task<Result<CloseSubscriptionResponse>> ExecuteAsync(int id, CloseSubscriptionRequest request)
        {
            var subscription = await context.Set<Subscriptions>()
                .Include(s => s.Vehicle)
                .Include(s => s.Rate)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription is null)
                return Result.NotFound("Subscription not found.");

            if (subscription.Status != SubscriptionStatus.Active)
                return Result.Conflict("Only active subscriptions can be closed.");

            // Buscar precio actual del plan
            var price = await context.Set<SubscriptionPrice>()
                .FirstOrDefaultAsync(p => p.VehicleType == subscription.Rate.VehicleType && p.Plan == subscription.Plan);
            if (price == null)
                return Result.NotFound("No price found for this plan and vehicle type.");

            // Buscar caja abierta
            var cash = await context.Set<CashRegister>()
                .FirstOrDefaultAsync(x => x.Status == CashStatus.Open);
            if (cash == null)
                return Result.Conflict("No open cash register.");

            // Registrar pago
            var payment = new Payment
            {
                Amount = price.Price,
                Method = request.Method,
                PaymentDate = dateTime.Now,
                CashRegisterId = cash.Id
            };
            context.Add(payment);

            // Cerrar suscripción
            subscription.Status = SubscriptionStatus.Cancelled;
            subscription.UpdatedAt = dateTime.Now;
            await context.SaveChangesAsync();

            return Result.Success(new CloseSubscriptionResponse
            {
                SubscriptionId = subscription.Id,
                Amount = price.Price,
                Plan = subscription.Plan,
                Method = request.Method,
                Message = $"Subscription closed and payment of {price.Price} registered via {request.Method}."
            });
        }
    }
}

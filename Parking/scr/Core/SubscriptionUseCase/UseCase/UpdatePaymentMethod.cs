using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class UpdateSubscriptionPaymentMethodRequest
    {
        public PaymentMethod Method { get; set; }
    }

    public class UpdateSubscriptionPaymentMethodResponse
    {
        public int PaymentId { get; set; }
        public PaymentMethod OldMethod { get; set; }
        public PaymentMethod NewMethod { get; set; }
        public decimal Amount { get; set; }
    }

    public class UpdateSubscriptionPaymentMethodUseCase(DbContext context)
    {
        public async Task<Result<UpdateSubscriptionPaymentMethodResponse>> ExecuteAsync(int subscriptionId, UpdateSubscriptionPaymentMethodRequest request)
        {
            var subscription = await context.Set<Subscriptions>().FindAsync(subscriptionId);

            if (subscription is null)
                return Result.NotFound("Subscription not found.");

            var payment = await context.Set<Payment>()
                .Where(p => p.ParkingSessionId == null)
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefaultAsync(p => p.Amount == subscription.Price && p.CashRegisterId ==
                    context.Set<CashRegister>()
                        .Where(c => c.Status == CashStatus.Open)
                        .Select(c => c.Id)
                        .FirstOrDefault());

            if (payment is null)
                return Result.NotFound("Payment not found for this subscription.");

            var cashRegister = await context.Set<CashRegister>()
                .FirstOrDefaultAsync(c => c.Id == payment.CashRegisterId);

            if (cashRegister != null && cashRegister.Status == CashStatus.Closed)
                return Result.Conflict("Cannot modify payment, cash register is already closed.");

            var oldMethod = payment.Method;
            payment.Method = request.Method;
            await context.SaveChangesAsync();

            return Result.Success(new UpdateSubscriptionPaymentMethodResponse
            {
                PaymentId = payment.Id,
                OldMethod = oldMethod,
                NewMethod = request.Method,
                Amount = payment.Amount
            });
        }
    }
}

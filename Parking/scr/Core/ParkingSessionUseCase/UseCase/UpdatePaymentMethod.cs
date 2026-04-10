using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase.UseCase
{
    public class UpdatePaymentMethodRequest
    {
        public PaymentMethod Method { get; set; }
    }

    public class UpdatePaymentMethodResponse
    {
        public int PaymentId { get; set; }
        public PaymentMethod OldMethod { get; set; }
        public PaymentMethod NewMethod { get; set; }
        public decimal Amount { get; set; }
    }

    public class UpdateParkingPaymentMethodUseCase(DbContext context)
    {
        public async Task<Result<UpdatePaymentMethodResponse>> ExecuteAsync(int sessionId, UpdatePaymentMethodRequest request)
        {
            var payment = await context.Set<Payment>()
                .FirstOrDefaultAsync(p => p.ParkingSessionId == sessionId);

            if (payment is null)
                return Result.NotFound("Payment not found for this parking session.");

            var cashRegister = await context.Set<CashRegister>()
                .FirstOrDefaultAsync(c => c.Id == payment.CashRegisterId);

            if (cashRegister != null && cashRegister.Status == CashStatus.Closed)
                return Result.Conflict("Cannot modify payment, cash register is already closed.");

            var oldMethod = payment.Method;
            payment.Method = request.Method;
            await context.SaveChangesAsync();

            return Result.Success(new UpdatePaymentMethodResponse
            {
                PaymentId = payment.Id,
                OldMethod = oldMethod,
                NewMethod = request.Method,
                Amount = payment.Amount
            });
        }
    }
}

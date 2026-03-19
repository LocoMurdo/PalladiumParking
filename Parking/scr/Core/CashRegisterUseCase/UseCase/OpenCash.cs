using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.CashRegisterUseCase.UseCase
{
    public class OpenCashRequest
    {
        public decimal OpeningAmount { get; init; }
    }
    public class OpenCashValidator : AbstractValidator<OpenCashRequest>
    {
        public OpenCashValidator()
        {
            RuleFor(x => x.OpeningAmount)
                .GreaterThanOrEqualTo(0);
        }
    }
    public class OpenCashUseCase(
    DbContext context,
    OpenCashValidator validator,
    IDatetimeservice dateTime)
    {
        public async Task<Result> ExecuteAsync(OpenCashRequest request)
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Result.Invalid(validation.Errors.Select(e => e.ErrorMessage));

            // 🔒 verificar si ya hay caja abierta
            var cashOpen = await context.Set<CashRegister>()
                .FirstOrDefaultAsync(x => x.Status == CashStatus.Open);

            if (cashOpen != null)
                return Result.Conflict("There is already an open cash register");

            var cash = new CashRegister
            {
                OpeningAmount = request.OpeningAmount,
                OpenedAt = dateTime.Now,
                Status = CashStatus.Open,
            };

            context.Add(cash);
            await context.SaveChangesAsync();

            return Result.Success("Cash opened successfully");
        }
    }
}

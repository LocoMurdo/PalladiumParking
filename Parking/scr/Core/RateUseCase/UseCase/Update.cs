using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.RateUseCase.UseCase
{
    public class UpdateRateRequest
    {
        public decimal PricePerHour { get; init; }
    }

    public class UpdateRateValidator : AbstractValidator<UpdateRateRequest>
    {
        public UpdateRateValidator()
        {
            RuleFor(r => r.PricePerHour)
                .GreaterThan(0)
                .WithMessage("Price per hour must be greater than zero.");
        }
    }

    public class UpdateRateUseCase(
        DbContext context,
        UpdateRateValidator validator)
    {
        public async Task<Result> ExecuteAsync(int id, UpdateRateRequest request)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Result.Failure(validation.Errors.Select(x => x.ErrorMessage));

            var rate = await context.Set<Rate>().FindAsync(id);
            if (rate is null)
                return Result.NotFound("Tarifa no encontrada.");

            rate.PricePerHour = request.PricePerHour;
            await context.SaveChangesAsync();

            return Result.Success("Tarifa actualizada correctamente.");
        }
    }
}

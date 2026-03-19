using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.RateUseCase.UseCase
{
    public class CreateRateRequest
    {
        public VehicleType VehicleType { get; init; }
        public decimal PricePerHour { get; init; }



        public Rate MapToRate() => new()
        {
            VehicleType = VehicleType,
            PricePerHour = PricePerHour,
            IsActive = true,

        };
    }

    public class createTypeValidator : AbstractValidator<CreateRateRequest>
    {
        public createTypeValidator()
        {
            RuleFor(x => x.VehicleType)
            .IsInEnum();

            RuleFor(request => request.PricePerHour)
                .GreaterThan(0)
                .WithMessage("Price per hour must be greater than zero.");
        }

    }

    public class CreateRateUseCase(
        DbContext context,
        createTypeValidator validator)
    {
        
        public async Task<Result> ExecuteAsync(CreateRateRequest request)
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Result.Failure(validation.Errors.Select(x => x.ErrorMessage));

            var exists = await context.Set<Rate>()
                .AnyAsync(r => r.VehicleType == request.VehicleType && r.IsActive);

            if (exists)
                return Result.Failure("Ya existe una tarifa activa para este tipo de vehículo.");

            var rate = new Rate
            {
                VehicleType = request.VehicleType,
                PricePerHour = request.PricePerHour,
                IsActive = true,
               
            };

            context.Add(rate);
            await context.SaveChangesAsync();

            return Result.Success("Tarifa creada correctamente.");
        }
    }
}


using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase.UseCase
{
    public class CreateSubscriptionRequest
    {
        public string LicensePlate { get; init; }
        public string? CarModel { get; init; }
        public VehicleType VehicleType { get; init; }
        public SubscriptionPlan Plan { get; init; }
    }

    public class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionRequest>
    {
        public CreateSubscriptionValidator()
        {
            RuleFor(r => r.LicensePlate)
                .NotEmpty()
                .MaximumLength(10);
            RuleFor(r => r.VehicleType)
                .IsInEnum();
            RuleFor(r => r.Plan)
                .IsInEnum();
        }
    }

    public class CreateSubscriptionResponse
    {
        public int SubscriptionId { get; init; }
        public string LicensePlate { get; init; }
        public string Plan { get; init; }
        public decimal Price { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public class CreateSubscriptionUseCase(
        DbContext context,
        CreateSubscriptionValidator validator,
        IDatetimeservice dateTime)
    {
        public async Task<Result<CreateSubscriptionResponse>> ExecuteAsync(CreateSubscriptionRequest request)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Result.Invalid(validation.Errors.Select(e => e.ErrorMessage));

            // Get subscription price
            var subscriptionPrice = await context.Set<SubscriptionPrice>()
                .FirstOrDefaultAsync(sp => sp.VehicleType == request.VehicleType && sp.Plan == request.Plan);

            if (subscriptionPrice is null)
                return Result.NotFound("No price found for this plan and vehicle type.");

            // Get rate for vehicle type
            var rate = await context.Set<Rate>()
                .FirstOrDefaultAsync(r => r.VehicleType == request.VehicleType && r.IsActive);

            if (rate is null)
                return Result.NotFound("No active rate found for this vehicle type.");

            // Find or create vehicle
            var vehicle = await context.Set<Vehicle>()
                .FirstOrDefaultAsync(v => v.PlateId == request.LicensePlate);

            if (vehicle is null)
            {
                vehicle = new Vehicle
                {
                    PlateId = request.LicensePlate,
                    CarModel = request.CarModel,
                    CreatedAt = dateTime.Now
                };
                context.Add(vehicle);
                await context.SaveChangesAsync();
            }

            // Check for active subscription on this vehicle
            var activeSubscription = await context.Set<Subscriptions>()
                .AnyAsync(s => s.VehicleId == vehicle.Id && s.Status == SubscriptionStatus.Active && s.EndDate > dateTime.Now);

            if (activeSubscription)
                return Result.Conflict("This vehicle already has an active subscription.");

            // Calculate dates
            var startDate = dateTime.Now;
            var endDate = request.Plan switch
            {
                SubscriptionPlan.Daily => startDate.AddDays(1),
                SubscriptionPlan.Biweekly => startDate.AddDays(15),
                SubscriptionPlan.Monthly => startDate.AddMonths(1),
                _ => startDate.AddDays(1)
            };

            var subscription = new Subscriptions
            {
                VehicleId = vehicle.Id,
                RateId = rate.Id,
                Plan = request.Plan,
                Price = subscriptionPrice.Price,
                StartDate = startDate,
                EndDate = endDate,
                Status = SubscriptionStatus.Active,
                CreatedAt = dateTime.Now
            };

            context.Add(subscription);
            await context.SaveChangesAsync();

            var response = new CreateSubscriptionResponse
            {
                SubscriptionId = subscription.Id,
                LicensePlate = vehicle.PlateId,
                Plan = request.Plan.ToString(),
                Price = subscriptionPrice.Price,
                StartDate = startDate,
                EndDate = endDate
            };

            return Result.Success(response, "Subscription created successfully.");
        }
    }
}

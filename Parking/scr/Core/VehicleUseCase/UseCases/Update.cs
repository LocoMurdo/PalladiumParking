using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.VehicleUseCase.UseCases
{
    public class UpdateVehicleRequest
    {
        public string LicensePlate { get; init; }
        public string CarModel { get; init; }
    }

    public class UpdateVehicleValidator : AbstractValidator<UpdateVehicleRequest>
    {
        public UpdateVehicleValidator()
        {
            RuleFor(r => r.LicensePlate)
                .NotEmpty()
                .MaximumLength(10);
            RuleFor(r => r.CarModel)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    public class UpdateVehicleUseCase(
        DbContext context,
        UpdateVehicleValidator validator,
        IDatetimeservice dateTime)
    {
        public async Task<Result> ExecuteAsync(int id, UpdateVehicleRequest request)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Result.Invalid(validation.Errors.Select(e => e.ErrorMessage));

            var vehicle = await context.Set<Vehicle>().FindAsync(id);
            if (vehicle is null)
                return Result.NotFound("Vehicle not found.");

            var duplicate = await context.Set<Vehicle>()
                .AnyAsync(v => v.PlateId == request.LicensePlate && v.Id != id);
            if (duplicate)
                return Result.Conflict("A vehicle with this plate already exists.");

            vehicle.PlateId = request.LicensePlate;
            vehicle.CarModel = request.CarModel;
            vehicle.UpdatedAt = dateTime.Now;
            await context.SaveChangesAsync();

            return Result.Success("Vehicle updated successfully.");
        }
    }
}

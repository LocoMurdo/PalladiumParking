using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;
using Parking.API.scr.Shared.Resource.ApiResponse;
using SimpleResults;

namespace Parking.API.scr.Core.VehicleUseCase.UseCases
{
    public class CreateVehicleRequest
    {
        public string LicensePlate { get; init; }
        public string Carmodel { get; init; }

        public Vehicle MapToVehicle(DateTime now) => new()
        {
            PlateId = LicensePlate,
            CarModel = Carmodel,
            CreatedAt = now
        };
    }

    public class CreateVehicleValidator : AbstractValidator<CreateVehicleRequest>
    {
        public CreateVehicleValidator()
        {
            RuleFor(request => request.LicensePlate)
                .NotEmpty()
                .MaximumLength(10);
            RuleFor(request => request.Carmodel)
                .NotEmpty()
                .MaximumLength(50);
          
        }
    }
    public class CreateVehicleUseCase(
        DbContext context,
        IVehicleRepository vehicleRepository,
        CreateVehicleValidator validator,
        IDatetimeservice dateTime)
    {
        public async Task<Result> ExecuteAsync(CreateVehicleRequest request)
        {
            var result = validator.Validate(request);
            if (result.IsFailed())
                return result.Invalid();

           if(await vehicleRepository.VehicleExistAsync(request.LicensePlate))
                return Result.Conflict(messages.VehicleAlreadyExists);
            var vehicle = request.MapToVehicle(dateTime.Now);
            context.Add(vehicle);
            await context.SaveChangesAsync();
            return Result.Success();
        }
    }
}

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Resource.ApiResponse;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase.UseCase
{
    public class CreateParkingSessionRequest
    {
        public string VisitorPlate { get; init; }
        public int RateId { get; init; }

        public ParkingSession MapToParkingSession(DateTime now) => new()
        {
            VisitorPlate = VisitorPlate,
            RateId = RateId,
            EntryTime = now,
            Status = ParkingSession.ParkingSessionStatus.Open
        };
    }

    public class CreateParkingSessionValidator : AbstractValidator<CreateParkingSessionRequest>
    {
        public CreateParkingSessionValidator()
        {
            RuleFor(request => request.VisitorPlate)
                .NotEmpty()
                .MaximumLength(10);
            RuleFor(request => request.RateId)
                .GreaterThan(0);
        }
    }

    public class CreateParkingSessionUseCase(
        DbContext context,
        CreateParkingSessionValidator validator,
        IDatetimeservice dateTime)
    {
        public async Task<Result> ExecuteAsync(CreateParkingSessionRequest request)
        {
            var validation = await validator.ValidateAsync(request);
            if (validation.IsFailed())
                return Result.Invalid(validation.Errors.Select(e => e.ErrorMessage));
            var parkingSession = request.MapToParkingSession(dateTime.Now);
            context.Add(parkingSession);
            await context.SaveChangesAsync();
            return Result.Success(messages.SuccessfulParkingSessionCreate);
        }   
    }
    }

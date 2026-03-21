using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Core.SubscriptionUseCase.UseCase;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Infrastructure.services;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;

namespace TestParking.UseCases
{
    public class SubscriptionUseCaseTest
    {
        private readonly IDatetimeservice _dateTime = new DateTimeService();

        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(new List<IEntityTypeConfigurator>(), options);

            context.Set<Rate>().Add(new Rate { Id = 1, VehicleType = VehicleType.Car, PricePerHour = 1000, IsActive = true });
            context.Set<Rate>().Add(new Rate { Id = 2, VehicleType = VehicleType.Motorcycle, PricePerHour = 800, IsActive = true });
            context.Set<SubscriptionPrice>().AddRange(
                new SubscriptionPrice { Id = 1, VehicleType = VehicleType.Car, Plan = SubscriptionPlan.Daily, Price = 3000 },
                new SubscriptionPrice { Id = 2, VehicleType = VehicleType.Car, Plan = SubscriptionPlan.Monthly, Price = 40000 },
                new SubscriptionPrice { Id = 3, VehicleType = VehicleType.Motorcycle, Plan = SubscriptionPlan.Daily, Price = 3000 }
            );
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task Create_ValidSubscription_ShouldSucceed()
        {
            var context = CreateContext();
            var validator = new CreateSubscriptionValidator();
            var useCase = new CreateSubscriptionUseCase(context, validator, _dateTime);

            var request = new CreateSubscriptionRequest
            {
                LicensePlate = "ABC123",
                CarModel = "Toyota",
                VehicleType = VehicleType.Car,
                Plan = SubscriptionPlan.Daily
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(3000, result.Data.Price);
            Assert.Equal("ABC123", result.Data.LicensePlate);
        }

        [Fact]
        public async Task Create_ShouldAutoCreateVehicle()
        {
            var context = CreateContext();
            var validator = new CreateSubscriptionValidator();
            var useCase = new CreateSubscriptionUseCase(context, validator, _dateTime);

            var request = new CreateSubscriptionRequest
            {
                LicensePlate = "NEW999",
                CarModel = "Honda",
                VehicleType = VehicleType.Car,
                Plan = SubscriptionPlan.Daily
            };

            await useCase.ExecuteAsync(request);

            var vehicle = await context.Set<Vehicle>().FirstOrDefaultAsync(v => v.PlateId == "NEW999");
            Assert.NotNull(vehicle);
            Assert.Equal("Honda", vehicle.CarModel);
        }

        [Fact]
        public async Task Create_DuplicateActiveSubscription_ShouldReturnConflict()
        {
            var context = CreateContext();
            var vehicle = new Vehicle { Id = 1, PlateId = "ABC123", CreatedAt = DateTime.Now };
            context.Set<Vehicle>().Add(vehicle);
            context.Set<Subscriptions>().Add(new Subscriptions
            {
                VehicleId = 1,
                RateId = 1,
                Plan = SubscriptionPlan.Monthly,
                Price = 40000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Status = SubscriptionStatus.Active
            });
            await context.SaveChangesAsync();

            var validator = new CreateSubscriptionValidator();
            var useCase = new CreateSubscriptionUseCase(context, validator, _dateTime);

            var request = new CreateSubscriptionRequest
            {
                LicensePlate = "ABC123",
                VehicleType = VehicleType.Car,
                Plan = SubscriptionPlan.Daily
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Create_EmptyPlate_ShouldReturnInvalid()
        {
            var context = CreateContext();
            var validator = new CreateSubscriptionValidator();
            var useCase = new CreateSubscriptionUseCase(context, validator, _dateTime);

            var request = new CreateSubscriptionRequest
            {
                LicensePlate = "",
                VehicleType = VehicleType.Car,
                Plan = SubscriptionPlan.Daily
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Cancel_ActiveSubscription_ShouldSucceed()
        {
            var context = CreateContext();
            context.Set<Subscriptions>().Add(new Subscriptions
            {
                Id = 1,
                VehicleId = 1,
                RateId = 1,
                Plan = SubscriptionPlan.Monthly,
                Price = 40000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Status = SubscriptionStatus.Active
            });
            context.Set<Vehicle>().Add(new Vehicle { Id = 1, PlateId = "ABC123" });
            await context.SaveChangesAsync();

            var useCase = new CancelSubscriptionUseCase(context);
            var result = await useCase.ExecuteAsync(1);

            Assert.True(result.IsSuccess);

            var sub = await context.Set<Subscriptions>().FindAsync(1);
            Assert.Equal(SubscriptionStatus.Cancelled, sub!.Status);
        }

        [Fact]
        public async Task Cancel_AlreadyCancelled_ShouldReturnConflict()
        {
            var context = CreateContext();
            context.Set<Subscriptions>().Add(new Subscriptions
            {
                Id = 1,
                VehicleId = 1,
                RateId = 1,
                Plan = SubscriptionPlan.Monthly,
                Price = 40000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Status = SubscriptionStatus.Cancelled
            });
            context.Set<Vehicle>().Add(new Vehicle { Id = 1, PlateId = "ABC123" });
            await context.SaveChangesAsync();

            var useCase = new CancelSubscriptionUseCase(context);
            var result = await useCase.ExecuteAsync(1);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CheckByPlate_ActiveSubscription_ShouldReturnTrue()
        {
            var context = CreateContext();
            context.Set<Vehicle>().Add(new Vehicle { Id = 1, PlateId = "SUB001" });
            context.Set<Subscriptions>().Add(new Subscriptions
            {
                VehicleId = 1,
                RateId = 1,
                Plan = SubscriptionPlan.Monthly,
                Price = 40000,
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(25),
                Status = SubscriptionStatus.Active
            });
            await context.SaveChangesAsync();

            var useCase = new CheckSubscriptionByPlateUseCase(context, _dateTime);
            var result = await useCase.ExecuteAsync("SUB001");

            Assert.True(result.IsSuccess);
            Assert.True(result.Data.HasActiveSubscription);
        }

        [Fact]
        public async Task CheckByPlate_NoSubscription_ShouldReturnFalse()
        {
            var context = CreateContext();
            var useCase = new CheckSubscriptionByPlateUseCase(context, _dateTime);
            var result = await useCase.ExecuteAsync("NOEXIST");

            Assert.True(result.IsSuccess);
            Assert.False(result.Data.HasActiveSubscription);
        }
    }
}

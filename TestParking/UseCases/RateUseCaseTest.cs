using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Core.RateUseCase.UseCase;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;

namespace TestParking.UseCases
{
    public class RateUseCaseTest
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(new List<IEntityTypeConfigurator>(), options);
        }

        [Fact]
        public async Task Create_ValidRate_ShouldSucceed()
        {
            var context = CreateContext();
            var validator = new createTypeValidator();
            var useCase = new CreateRateUseCase(context, validator);

            var request = new CreateRateRequest
            {
                VehicleType = VehicleType.Car,
                PricePerHour = 1000
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Single(context.Set<Rate>());
        }

        [Fact]
        public async Task Create_DuplicateActiveRate_ShouldFail()
        {
            var context = CreateContext();
            context.Set<Rate>().Add(new Rate { VehicleType = VehicleType.Car, PricePerHour = 1000, IsActive = true });
            await context.SaveChangesAsync();

            var validator = new createTypeValidator();
            var useCase = new CreateRateUseCase(context, validator);

            var request = new CreateRateRequest
            {
                VehicleType = VehicleType.Car,
                PricePerHour = 1500
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Create_ZeroPrice_ShouldReturnInvalid()
        {
            var context = CreateContext();
            var validator = new createTypeValidator();
            var useCase = new CreateRateUseCase(context, validator);

            var request = new CreateRateRequest
            {
                VehicleType = VehicleType.Car,
                PricePerHour = 0
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Update_ExistingRate_ShouldUpdatePrice()
        {
            var context = CreateContext();
            context.Set<Rate>().Add(new Rate { Id = 1, VehicleType = VehicleType.Car, PricePerHour = 1000, IsActive = true });
            await context.SaveChangesAsync();

            var validator = new UpdateRateValidator();
            var useCase = new UpdateRateUseCase(context, validator);

            var result = await useCase.ExecuteAsync(1, new UpdateRateRequest { PricePerHour = 1500 });

            Assert.True(result.IsSuccess);

            var rate = await context.Set<Rate>().FindAsync(1);
            Assert.Equal(1500, rate!.PricePerHour);
        }

        [Fact]
        public async Task Update_NonExistent_ShouldReturnNotFound()
        {
            var context = CreateContext();
            var validator = new UpdateRateValidator();
            var useCase = new UpdateRateUseCase(context, validator);

            var result = await useCase.ExecuteAsync(999, new UpdateRateRequest { PricePerHour = 1500 });

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Delete_ExistingRate_ShouldDeactivate()
        {
            var context = CreateContext();
            context.Set<Rate>().Add(new Rate { Id = 1, VehicleType = VehicleType.Car, PricePerHour = 1000, IsActive = true });
            await context.SaveChangesAsync();

            var useCase = new DeleteRateUseCase(context);
            var result = await useCase.ExecuteAsync(1);

            Assert.True(result.IsSuccess);

            var rate = await context.Set<Rate>().FindAsync(1);
            Assert.False(rate!.IsActive);
        }

        [Fact]
        public async Task GetAll_WithRates_ShouldReturnList()
        {
            var context = CreateContext();
            context.Set<Rate>().Add(new Rate { VehicleType = VehicleType.Car, PricePerHour = 1000, IsActive = true });
            context.Set<Rate>().Add(new Rate { VehicleType = VehicleType.Motorcycle, PricePerHour = 800, IsActive = true });
            await context.SaveChangesAsync();

            var useCase = new GetAllRatesUseCase(context);
            var result = await useCase.ExecuteAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetAll_Empty_ShouldReturnEmptyList()
        {
            var context = CreateContext();
            var useCase = new GetAllRatesUseCase(context);
            var result = await useCase.ExecuteAsync();

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}

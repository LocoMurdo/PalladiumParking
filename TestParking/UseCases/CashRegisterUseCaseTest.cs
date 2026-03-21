using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Core.CashRegisterUseCase.UseCase;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Infrastructure.services;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;

namespace TestParking.UseCases
{
    public class CashRegisterUseCaseTest
    {
        private readonly IDatetimeservice _dateTime = new DateTimeService();

        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(new List<IEntityTypeConfigurator>(), options);
        }

        [Fact]
        public async Task Open_NoCashOpen_ShouldSucceed()
        {
            var context = CreateContext();
            var validator = new OpenCashValidator();
            var useCase = new OpenCashUseCase(context, validator, _dateTime);

            var result = await useCase.ExecuteAsync(new OpenCashRequest { OpeningAmount = 50000 });

            Assert.True(result.IsSuccess);

            var cash = await context.Set<CashRegister>().FirstOrDefaultAsync();
            Assert.NotNull(cash);
            Assert.Equal(50000, cash.OpeningAmount);
            Assert.Equal(CashStatus.Open, cash.Status);
        }

        [Fact]
        public async Task Open_AlreadyOpen_ShouldReturnConflict()
        {
            var context = CreateContext();
            context.Set<CashRegister>().Add(new CashRegister
            {
                OpeningAmount = 50000,
                Status = CashStatus.Open,
                OpenedAt = DateTime.Now
            });
            await context.SaveChangesAsync();

            var validator = new OpenCashValidator();
            var useCase = new OpenCashUseCase(context, validator, _dateTime);

            var result = await useCase.ExecuteAsync(new OpenCashRequest { OpeningAmount = 30000 });

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Close_NoCashOpen_ShouldReturnConflict()
        {
            var context = CreateContext();
            var useCase = new CloseCashUseCase(context, _dateTime);

            var result = await useCase.ExecuteAsync();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Close_WithOpenSessions_ShouldReturnConflict()
        {
            var context = CreateContext();
            context.Set<CashRegister>().Add(new CashRegister
            {
                Id = 1,
                OpeningAmount = 50000,
                Status = CashStatus.Open,
                OpenedAt = DateTime.Now
            });
            context.Set<Rate>().Add(new Rate { Id = 1, PricePerHour = 1000, IsActive = true });
            context.Set<ParkingSession>().Add(new ParkingSession
            {
                VisitorPlate = "ABC123",
                RateId = 1,
                EntryTime = DateTime.Now,
                Status = ParkingSession.ParkingSessionStatus.Open
            });
            await context.SaveChangesAsync();

            var useCase = new CloseCashUseCase(context, _dateTime);
            var result = await useCase.ExecuteAsync();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Close_NoOpenSessions_ShouldSucceed()
        {
            var context = CreateContext();
            context.Set<CashRegister>().Add(new CashRegister
            {
                Id = 1,
                OpeningAmount = 50000,
                Status = CashStatus.Open,
                OpenedAt = DateTime.Now
            });
            await context.SaveChangesAsync();

            var useCase = new CloseCashUseCase(context, _dateTime);
            var result = await useCase.ExecuteAsync();

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);

            var cash = await context.Set<CashRegister>().FindAsync(1);
            Assert.Equal(CashStatus.Closed, cash!.Status);
        }
    }
}

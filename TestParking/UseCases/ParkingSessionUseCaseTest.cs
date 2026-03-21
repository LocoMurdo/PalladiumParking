using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Core.ParkingSessionUseCase.UseCase;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Infrastructure.services;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;

namespace TestParking.UseCases
{
    public class ParkingSessionUseCaseTest
    {
        private readonly AppDbContext _context;
        private readonly IDatetimeservice _dateTime;

        public ParkingSessionUseCaseTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var configurators = new List<IEntityTypeConfigurator>();
            _context = new AppDbContext(configurators, options);
            _dateTime = new DateTimeService();

            _context.Set<Rate>().Add(new Rate { Id = 1, VehicleType = VehicleType.Car, PricePerHour = 1000, IsActive = true });
            _context.SaveChanges();
        }

        [Fact]
        public async Task Create_ValidRequest_ShouldCreateSession()
        {
            var validator = new CreateParkingSessionValidator();
            var useCase = new CreateParkingSessionUseCase(_context, validator, _dateTime);

            var request = new CreateParkingSessionRequest
            {
                VisitorPlate = "ABC123",
                RateId = 1
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.True(result.IsSuccess);

            var session = await _context.Set<ParkingSession>().FirstOrDefaultAsync();
            Assert.NotNull(session);
            Assert.Equal("ABC123", session.VisitorPlate);
            Assert.Equal(ParkingSession.ParkingSessionStatus.Open, session.Status);
        }

        [Fact]
        public async Task Create_EmptyPlate_ShouldReturnInvalid()
        {
            var validator = new CreateParkingSessionValidator();
            var useCase = new CreateParkingSessionUseCase(_context, validator, _dateTime);

            var request = new CreateParkingSessionRequest
            {
                VisitorPlate = "",
                RateId = 1
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Create_InvalidRateId_ShouldReturnInvalid()
        {
            var validator = new CreateParkingSessionValidator();
            var useCase = new CreateParkingSessionUseCase(_context, validator, _dateTime);

            var request = new CreateParkingSessionRequest
            {
                VisitorPlate = "ABC123",
                RateId = 0
            };

            var result = await useCase.ExecuteAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Cancel_OpenSession_ShouldCancel()
        {
            _context.Set<ParkingSession>().Add(new ParkingSession
            {
                Id = 1,
                VisitorPlate = "ABC123",
                RateId = 1,
                EntryTime = DateTime.Now,
                Status = ParkingSession.ParkingSessionStatus.Open
            });
            await _context.SaveChangesAsync();

            var useCase = new CancelParkingSessionUseCase(_context);
            var result = await useCase.ExecuteAsync(1);

            Assert.True(result.IsSuccess);

            var session = await _context.Set<ParkingSession>().FindAsync(1);
            Assert.Equal(ParkingSession.ParkingSessionStatus.Cancelled, session!.Status);
        }

        [Fact]
        public async Task Cancel_ClosedSession_ShouldReturnConflict()
        {
            _context.Set<ParkingSession>().Add(new ParkingSession
            {
                Id = 2,
                VisitorPlate = "XYZ789",
                RateId = 1,
                EntryTime = DateTime.Now,
                Status = ParkingSession.ParkingSessionStatus.Closed
            });
            await _context.SaveChangesAsync();

            var useCase = new CancelParkingSessionUseCase(_context);
            var result = await useCase.ExecuteAsync(2);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Cancel_NonExistent_ShouldReturnNotFound()
        {
            var useCase = new CancelParkingSessionUseCase(_context);
            var result = await useCase.ExecuteAsync(999);

            Assert.False(result.IsSuccess);
        }
    }
}

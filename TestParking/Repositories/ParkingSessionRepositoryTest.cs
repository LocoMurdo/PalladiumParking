using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Infrastructure.Persistence.Repositories;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestParking.Repositories
{
    public class ParkingSessionRepositoryTest
    {
        [Fact]
        public async Task ShouldCalculateParkingTotal()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ParkingTest")
                .Options;
            var configurators = new List<IEntityTypeConfigurator>();

            var context = new AppDbContext(configurators,options);


            context.Add(new Rate
            {
                Id = 1,
                PricePerHour = 1000
            });
            var entry = new DateTime(2024, 1, 1, 10, 0, 0);
            var exit = new DateTime(2024, 1, 1, 12, 0, 0);

            await context.SaveChangesAsync();

            var repository = new ParkingSessionRepository(context);

            var session = new ParkingSession
            {
                RateId = 1,
                EntryTime = entry,
                ExitTime = exit
            };

            var total = await repository.CalculateTotalAsync(session);

            Assert.Equal(2000, total);
        }
    }
}

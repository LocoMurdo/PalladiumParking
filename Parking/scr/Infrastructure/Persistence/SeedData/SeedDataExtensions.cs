using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;

namespace Parking.API.scr.Infrastructure.Persistence.SeedData
{
    public static class SeedDataExtensions
    {
        /// <summary>
        /// Seeds the default rates for cars and motorcycles.
        /// </summary>
        public static ModelBuilder SeedRates(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>().HasData(
                new Rate
                {
                    Id = 1,
                    VehicleType = VehicleType.Car,
                    PricePerHour = 1000.00m,
                    IsActive = true
                },
                new Rate
                {
                    Id = 2,
                    VehicleType = VehicleType.Motorcycle,
                    PricePerHour = 800.00m,
                    IsActive = true
                }
            );

            return modelBuilder;
        }

        /// <summary>
        /// Seeds the default subscription prices.
        /// </summary>
        public static ModelBuilder SeedSubscriptionPrices(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionPrice>().HasData(
                // Car prices
                new SubscriptionPrice { Id = 1, VehicleType = VehicleType.Car, Plan = SubscriptionPlan.Daily, Price = 3000.00m },
                new SubscriptionPrice { Id = 2, VehicleType = VehicleType.Car, Plan = SubscriptionPlan.Biweekly, Price = 20000.00m },
                new SubscriptionPrice { Id = 3, VehicleType = VehicleType.Car, Plan = SubscriptionPlan.Monthly, Price = 40000.00m },
                // Motorcycle prices
                new SubscriptionPrice { Id = 4, VehicleType = VehicleType.Motorcycle, Plan = SubscriptionPlan.Daily, Price = 2000.00m },
                new SubscriptionPrice { Id = 5, VehicleType = VehicleType.Motorcycle, Plan = SubscriptionPlan.Biweekly, Price = 15000.00m },
                new SubscriptionPrice { Id = 6, VehicleType = VehicleType.Motorcycle, Plan = SubscriptionPlan.Monthly, Price = 30000.00m }
            );

            return modelBuilder;
        }
    }
}

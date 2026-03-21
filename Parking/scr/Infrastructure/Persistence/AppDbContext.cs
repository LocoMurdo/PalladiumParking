using Microsoft.EntityFrameworkCore;
using Parking.API.scr.HostAplication.Extensions;
using Parking.API.scr.Infrastructure.Persistence.SeedData;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using System;
using System.Data;
using System.Reflection;

namespace Parking.API.scr.Infrastructure.Persistence
{
    public class AppDbContext(
        IEnumerable<IEntityTypeConfigurator> entityTypeConfigurators,
       //IWebHostEnvironment env,
       DbContextOptions<AppDbContext> options) : DbContext(options)
    {
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Execute the entity configurations that expose the plugins.
            foreach (IEntityTypeConfigurator entityTypeConfigurator in entityTypeConfigurators)
            {
                entityTypeConfigurator.ConfigureEntities(modelBuilder);
            }

            modelBuilder
            .AddEntity<Person>()
             .AddEntity<User>()
            .AddEntity<Vehicle>()
            .AddEntity<Rate>()
            .AddEntity<ParkingSession>()
            .AddEntity<Payment>()
            .AddEntity<CashRegister>()
            .AddEntity<CashMovement>()
            .AddEntity<Subscriptions>()
            .AddEntity<SubscriptionPrice>()
            .AddEntity<RefreshToken>();

            // Seed data
            modelBuilder
                .SeedRates()
                .SeedSubscriptionPrices();



            // dont use for now 


            /*modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            if (!env.IsEnvironment("Test"))
            {
                modelBuilder
                .CreateDefaultRoles()
                .CreateDefaultGenders();
                //theres ones not used yet 
                // .CreateDefaultKinships()
                //.CreateDefaultAppointmentStatus()
                // .CreateDefaultWeekDays()

            }

            if (env.IsDevelopment())
            {

                modelBuilder


                  .CreateDefaultWeekDays()

                  .CreateDefaultGeneralTreatments()
                  .CreateDefaultOffices()
                  .CreateDefaultOfficeSchedules()
                  // .CreateDefaultSpecificTreatments()
                  //  .CreateDefaultEmployeeSchedules()
                  .CreateDefaultUserAccounts();

            }*/

        }
    }
}


































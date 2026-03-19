using Microsoft.AspNetCore.Identity;
using Parking.API.scr.Infrastructure.Persistence.Repositories;
using Parking.API.scr.Infrastructure.services.Token;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;

namespace Parking.API.scr.Infrastructure.services
{
    public static class  InfraestructureServicesExtensions
    {

        public static IServiceCollection AddInfraestructureServices(this IServiceCollection services) {

            //services for repository
            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IVehicleRepository, VehicleRepository>()
                .AddScoped<IParkingSessionRepository, ParkingSessionRepository>();

            services.
                AddScoped<ITokenService, TokenService>()
            .AddScoped<IDatetimeservice, DateTimeService>()
          // .AddScoped<ICurrentEmployee, CurrentEmployeeService>()
          .AddSingleton<IPasswordHasher, PasswordHasherBcrypt>();




            return services;
        }

    }
}
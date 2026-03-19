using CPlugin.Net;
using FluentValidation;

namespace Parking.API.scr.HostAplication.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(PluginLoader.Assemblies, ServiceLifetime.Transient);
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            // .AddValidatorsFromAssembly(typeof(CoreServicesExtensions).Assembly, ServiceLifetime.Transient);

            return services;
        }
    }
}

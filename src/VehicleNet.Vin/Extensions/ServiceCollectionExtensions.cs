using Microsoft.Extensions.DependencyInjection;
using VehicleNet.Vin.Interfaces;
using VehicleNet.Vin.Services;

namespace VehicleNet.Vin.Extensions;

/// <summary>
/// Provides extension methods for registering VIN-related services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the VIN-related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to which the VIN-related services will be added.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddVehicleVinServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IVinValidator, VinValidatorService>();
        services.AddSingleton<IVinParser, VinParserService>();
        services.AddSingleton<IVinGenerator, VinGeneratorService>();

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using VehicleNet.Vin.Interfaces;
using VehicleNet.Vin.Services;

namespace VehicleNet.Vin.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVehicleVinServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IVinValidator, VinValidatorService>();
        services.AddSingleton<IVinParser, VinParserService>();
        services.AddSingleton<IVinGenerator, VinGeneratorService>();

        return services;
    }
}

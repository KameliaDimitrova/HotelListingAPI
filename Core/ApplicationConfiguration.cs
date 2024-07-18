using HotelListingAPI.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddServices()
            .AddRepositories();

    internal static IServiceCollection AddServices(this IServiceCollection services)
        => services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(calsses => calsses.AssignableTo(typeof(IService)))
            .AsMatchingInterface()
            .WithTransientLifetime());

    internal static IServiceCollection AddRepositories(this IServiceCollection services)
        => services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(calsses => calsses.AssignableTo(typeof(IRepository<>)))
            .AsMatchingInterface()
            .WithTransientLifetime());
}

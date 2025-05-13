using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Mediator.UnitTests.Mocks;

internal static class ServiceProviderFactory
{
    public static IServiceProvider CreateWith(Type service, Type implementation)
    {
        var services = new ServiceCollection();
        services.AddScoped(service, implementation);

        return services.BuildServiceProvider();
    }

    [ExcludeFromCodeCoverage]
    public record ServiceTypes(Type Service, Type Implementation);

    public static IServiceProvider CreateWith(IEnumerable<ServiceTypes> serviceTypes)
    {
        var services = new ServiceCollection();
        foreach (var t in serviceTypes)
        {
            services.AddScoped(t.Service, t.Implementation);
        }

        return services.BuildServiceProvider();
    }
}

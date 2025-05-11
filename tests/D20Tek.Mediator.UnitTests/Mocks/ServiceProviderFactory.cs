using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator.UnitTests.Mocks;

internal static class ServiceProviderFactory
{
    public static IServiceProvider CreateWith(Type service, Type implementation)
    {
        var services = new ServiceCollection();
        services.AddScoped(service, implementation);

        return services.BuildServiceProvider();
    }
}

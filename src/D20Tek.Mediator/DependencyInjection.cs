using D20Tek.Functional;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace D20Tek.Mediator;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        Assembly[] assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) =>
        services.AddRequiredServices(lifetime)
                .AddTypesForAssemblies(assemblies, lifetime);

    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        Assembly? assembly = null,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) =>
        services.AddMediator(assembly is null ? [] : [assembly], lifetime);

    public static IServiceCollection AddMediatorFor<T>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) =>
        services.AddMediator([typeof(T).Assembly], lifetime);

    private static IServiceCollection AddRequiredServices(
        this IServiceCollection services,
        ServiceLifetime lifetime) =>
        services.Pipe(s => s.TryAdd(new ServiceDescriptor(typeof(IMediator), typeof(Mediator), lifetime)));
}

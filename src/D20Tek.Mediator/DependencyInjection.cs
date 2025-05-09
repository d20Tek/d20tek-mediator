using D20Tek.Functional;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace D20Tek.Mediator;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        Assembly[]? assemblies = null,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) =>
        services.AddRequiredServices(lifetime)
                .AddTypesForAssemblies(assemblies ?? [], lifetime);

    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        Assembly? assembly = null,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) =>
        services.AddMediator(assembly is null ? [] : [assembly], lifetime);

    public static IServiceCollection AddMediatorFor<T>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) =>
        services.AddMediator([typeof(T).Assembly], lifetime);

    private static IServiceCollection AddRequiredServices(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.TryAdd(new ServiceDescriptor(typeof(IMediator), typeof(Mediator), lifetime));
        return services;
    }

    private static IServiceCollection AddTypesForAssemblies(
        this IServiceCollection services,
        Assembly[] assemblies,
        ServiceLifetime lifetime)
    {
        assemblies.ForEach(assembly => services.AddRequestHandlersFromAssembly(assembly, lifetime));
        return services;
    }

    private static readonly Type[] _handlerInterfaceTypes = 
    [
        typeof(IRequestHandlerAsync<,>),
        typeof(IRequestHandlerAsync<>)
    ];

    private static IServiceCollection AddRequestHandlersFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime)
    {
        assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface)
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && _handlerInterfaceTypes.Contains(i.GetGenericTypeDefinition()))
                    .Select(i => new { Interface = i, Implementation = type }))
                .ForEach(handler => services.TryAdd(
                    new ServiceDescriptor(handler.Interface, handler.Implementation, lifetime)));

        return services;
    }
}

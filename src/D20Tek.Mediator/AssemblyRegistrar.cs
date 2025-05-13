using D20Tek.Functional;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace D20Tek.Mediator;

internal static class AssemblyRegistrar
{
    private static readonly Type[] _handlerInterfaceTypes =
    [
        typeof(ICommandHandlerAsync<,>),
        typeof(ICommandHandlerAsync<>),
        typeof(ICommandHandler<,>),
        typeof(ICommandHandler<>),
        typeof(INotificationHandlerAsync<>),
        typeof(INotificationHandler<>)
    ];

    internal static IServiceCollection AddTypesForAssemblies(
        this IServiceCollection services,
        Assembly[] assemblies,
        ServiceLifetime lifetime) =>
        services.Pipe(s => assemblies.ForEach(assembly => s.AddCommandHandlersFromAssembly(assembly, lifetime)));

    private static IServiceCollection AddCommandHandlersFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime) =>
        services.Pipe(s => assembly
            .GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && _handlerInterfaceTypes.Contains(i.GetGenericTypeDefinition()))
                .Select(i => new { Interface = i, Implementation = type }))
            .ForEach(handler => s.Add(
                new ServiceDescriptor(handler.Interface, handler.Implementation, lifetime))));
}

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace D20Tek.Mediator;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly? assembly = null)
    {
        services.AddScoped<IMediator, Mediator>();
        //services.AddRequestHandlersFromAssembly(assembly ?? Assembly.GetCallingAssembly());
        return services;
    }

    private static IServiceCollection AddRequestHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaceType = typeof(IRequestHandlerAsync<,>);

        var handlerTypes = assembly
            .GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                .Select(i => new { Interface = i, Implementation = type }));

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.Interface, handler.Implementation);
        }

        return services;
    }
}

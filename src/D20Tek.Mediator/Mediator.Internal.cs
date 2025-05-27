using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator;

public partial class Mediator
{
    private (object Instance, Type Type) GetHandler(Type typeInterface, object command, Type? typeResponse = null)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            command.GetType(),
            et => typeResponse is null ?
                    typeInterface.MakeGenericType(command.GetType()) :
                    typeInterface.MakeGenericType(command.GetType(), typeResponse));

        object handler = _provider.GetRequiredService(handlerType);
        return (handler, handlerType);
    }

    private (object[] Instances, Type Type) GetNotificationHandlers(Type typeInterface, object notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            notification.GetType(),
            et => typeInterface.MakeGenericType(et));

        var handlers = _provider.GetServices(handlerType);

        object[] instances = [.. handlers.OfType<object>()];
        return (instances, handlerType);
    }

    internal static object? InvokeHandler(
        object handler, Type handlerType, string methodName, object[] parameters, bool voidExpected = false)
    {
        var method = handlerType.GetMethod(methodName)
            ?? throw new InvalidOperationException(
                $"Handler for {handlerType.Name} does not contain {methodName} method");

        var result = method.Invoke(handler, parameters);
        if (voidExpected is false && result is null)
            throw new InvalidOperationException($"Invocation of {handlerType.Name}.{methodName} returned null");

        return result;
    }
}

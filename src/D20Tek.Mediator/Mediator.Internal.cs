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
}

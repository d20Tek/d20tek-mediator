using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator;

public partial class Mediator
{
    private object GetCommandHandler(Type typeInterface, object command, Type? typeResponse = null)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            command.GetType(),
            et => typeResponse is null ?
                    typeInterface.MakeGenericType(command.GetType()) :
                    typeInterface.MakeGenericType(command.GetType(), typeResponse));

        return _provider.GetRequiredService(handlerType);
    }

    private object[] GetNotificationHandlers(Type typeInterface, object notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            notification.GetType(),
            et => typeInterface.MakeGenericType(et));

        var handlers = _provider.GetServices(handlerType);
        return [.. handlers.OfType<object>()];
    }
}

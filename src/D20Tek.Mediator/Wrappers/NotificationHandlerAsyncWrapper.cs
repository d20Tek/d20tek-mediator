using System.Collections.Concurrent;

namespace D20Tek.Mediator.Wrappers;

internal abstract class NotificationHandlerAsyncWrapper
{
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public abstract Task HandleAsync(INotification notification, CancellationToken cancellationToken);

    public static NotificationHandlerAsyncWrapper Create(object handler, Type notificationType)
    {
        var wrapperType = WrapperTypeDictionary.GetOrAdd(
            notificationType,
            et => typeof(NotificationHandlerAsyncWrapper<>).MakeGenericType(et));

        return (NotificationHandlerAsyncWrapper)Activator.CreateInstance(wrapperType, handler)!;
    }
}

// Generic wrapper that provides strong typing for handler invocation
internal sealed class NotificationHandlerAsyncWrapper<T>(object handler) : NotificationHandlerAsyncWrapper
    where T : INotification
{
    private readonly INotificationHandlerAsync<T> _handler = (INotificationHandlerAsync<T>)handler;

    public override async Task HandleAsync(
        INotification notification,
        CancellationToken cancellationToken)
    {
        await _handler.HandleAsync((T)notification, cancellationToken);
    }
}

using System.Collections.Concurrent;

namespace D20Tek.Mediator.Wrappers;

internal abstract class NotificationHandlerWrapper
{
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public abstract void Handle(INotification notification);

    public static NotificationHandlerWrapper Create(object handler, Type notificationType)
    {
        var wrapperType = WrapperTypeDictionary.GetOrAdd(
            notificationType,
            et => typeof(NotificationHandlerWrapper<>).MakeGenericType(et));

        return (NotificationHandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
    }
}

// Generic wrapper that provides strong typing for handler invocation
internal sealed class NotificationHandlerWrapper<T>(object handler) : NotificationHandlerWrapper
    where T : INotification
{
    private readonly INotificationHandler<T> _handler = (INotificationHandler<T>)handler;

    public override void Handle(INotification notification) =>
        _handler.Handle((T)notification);
}

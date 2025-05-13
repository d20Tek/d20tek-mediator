namespace D20Tek.Mediator;

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    void Handle(TNotification notification);
}

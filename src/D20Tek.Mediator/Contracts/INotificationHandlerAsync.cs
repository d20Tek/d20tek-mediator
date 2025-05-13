namespace D20Tek.Mediator;

public interface INotificationHandlerAsync<in TNotification>
    where TNotification : INotification
{
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}

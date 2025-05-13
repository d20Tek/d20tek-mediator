namespace MemberService.Endpoints.Members.MemberNotifications;

internal class SendWelcomeText : INotificationHandlerAsync<MemberCreatedNotification>
{
    public Task HandleAsync(MemberCreatedNotification notification, CancellationToken cancellationToken)
    {
        if (notification.CellPhone is null)
        {
            Console.WriteLine("This new member does not have a cell phone number in our system.");
        }
        else
        {
            Console.WriteLine($"Text sent to {notification.CellPhone}");
        }
        return Task.CompletedTask;
    }
}

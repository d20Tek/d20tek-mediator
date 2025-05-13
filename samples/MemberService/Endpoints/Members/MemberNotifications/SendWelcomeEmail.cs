namespace MemberService.Endpoints.Members.MemberNotifications;

internal class SendWelcomeEmail : INotificationHandlerAsync<MemberCreatedNotification>
{
    public Task HandleAsync(MemberCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Email sent to {notification.Email}");
        Console.WriteLine($"Dear {notification.Name}, Welcome to our service! You can start using it at any time.");
        return Task.CompletedTask;
    }
}

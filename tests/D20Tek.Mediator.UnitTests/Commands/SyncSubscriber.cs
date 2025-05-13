using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Mediator.UnitTests.Commands;

internal class SyncSubscriber
{
    [ExcludeFromCodeCoverage]
    public record TestEvent : INotification;

    public class Handler1 : INotificationHandler<TestEvent>
    {
        public void Handle(TestEvent notification)
        {
            Console.WriteLine("Handler1.Handle called.");
        }
    }

    public class Handler2 : INotificationHandler<TestEvent>
    {
        public void Handle(TestEvent notification)
        {
            Console.WriteLine("Handler2.Handle called.");
        }
    }
}

using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Mediator.UnitTests.Commands;

internal class AsyncSubscriber
{
    [ExcludeFromCodeCoverage]
    public record TestEvent : INotification;

    public class Handler1 : INotificationHandlerAsync<TestEvent>
    {
        public Task HandleAsync(TestEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("Handler1.HandleAsync called.");
            return Task.CompletedTask;
        }
    }

    public class Handler2 : INotificationHandlerAsync<TestEvent>
    {
        public Task HandleAsync(TestEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("Handler2.HandleAsync called.");
            return Task.CompletedTask;
        }
    }
}

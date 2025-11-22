namespace D20Tek.Mediator.UnitTests;

[TestClass]
public class MediatorNotificationTests
{
    public MediatorNotificationTests()
    {
        ConsoleMock.Initialize();   
    }

    [TestMethod]
    public async Task NotifyAsync_WithTestNotification_ReturnsTask()
    {
        // arrange
        ServiceProviderFactory.ServiceTypes[] handlerTypes =
        [
            new(typeof(INotificationHandlerAsync<AsyncSubscriber.TestEvent>), typeof(AsyncSubscriber.Handler1)),
            new(typeof(INotificationHandlerAsync<AsyncSubscriber.TestEvent>), typeof(AsyncSubscriber.Handler2))
        ];

        var provider = ServiceProviderFactory.CreateWith(handlerTypes);
        var mediator = new Mediator(provider);

        // act
        var task = mediator.NotifyAsync(new AsyncSubscriber.TestEvent(), CancellationToken.None);
        await task;

        // assert
        Assert.IsNotNull(task);
        Assert.IsTrue(task.IsCompleted);

        string consoleOutput = ConsoleMock.GetOutput();
        if (string.IsNullOrEmpty(consoleOutput) is false)
        {
            Assert.Contains("Handler1.HandleAsync called", consoleOutput);
            Assert.Contains("Handler2.HandleAsync called", consoleOutput);
        }
    }

    [TestMethod]
    public void Notify_WithTestNotification_Returns()
    {
        // arrange
        ServiceProviderFactory.ServiceTypes[] handlerTypes =
        [
            new(typeof(INotificationHandler<SyncSubscriber.TestEvent>), typeof(SyncSubscriber.Handler1)),
            new(typeof(INotificationHandler<SyncSubscriber.TestEvent>), typeof(SyncSubscriber.Handler2))
        ];

        var provider = ServiceProviderFactory.CreateWith(handlerTypes);
        var mediator = new Mediator(provider);

        // act
        mediator.Notify(new SyncSubscriber.TestEvent());

        // assert
        string consoleOutput = ConsoleMock.GetOutput();
        if (string.IsNullOrEmpty(consoleOutput) is false)
        {
            Assert.Contains("Handler1.Handle called", consoleOutput);
            Assert.Contains("Handler2.Handle called", consoleOutput);
        }
    }
}

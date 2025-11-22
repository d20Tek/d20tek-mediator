namespace D20Tek.Mediator.UnitTests;

[TestClass]
public sealed class MediatorTests
{
    [TestMethod]
    public void Send_WithCommandResponse_ReturnsSuccess()
    {
        // arrange
        var provider = ServiceProviderFactory.CreateWith(
                        typeof(ICommandHandler<SyncWithResponse.Command, SyncWithResponse.Response>),
                        typeof(SyncWithResponse.Handler));
        var mediator = new Mediator(provider);

        // act
        var result = mediator.Send(new SyncWithResponse.Command(true));

        // assert
        Assert.AreEqual("success", result.Value);
    }

    [TestMethod]
    public void Send_WithCommandResponse_ReturnsFailed()
    {
        // arrange
        var provider = ServiceProviderFactory.CreateWith(
                        typeof(ICommandHandler<SyncWithResponse.Command, SyncWithResponse.Response>),
                        typeof(SyncWithResponse.Handler));
        var mediator = new Mediator(provider);

        // act
        var result = mediator.Send(new SyncWithResponse.Command(false));

        // assert
        Assert.AreEqual("failed", result.Value);
    }

    [TestMethod]
    public void Send_WithCommandNoResponse_ReturnsTask()
    {
        // arrange
        var provider = ServiceProviderFactory.CreateWith(
                        typeof(ICommandHandler<SyncWithNoResponse.Command>),
                        typeof(SyncWithNoResponse.Handler));
        var mediator = new Mediator(provider);

        // act
        mediator.Send(new SyncWithNoResponse.Command());
    }

    [TestMethod]
    public void Send_WithSyncAndAsyncHandlers_ThrowsException()
    {
        // arrange
        ServiceProviderFactory.ServiceTypes[] handlerTypes =
        [
            new(typeof(ICommandHandler<CommonWithNoResponse.Command>), typeof(CommonWithNoResponse.Handler)),
            new(typeof(ICommandHandlerAsync<CommonWithNoResponse.Command>), typeof(CommonWithNoResponse.HandlerAsync)),
        ];

        var provider = ServiceProviderFactory.CreateWith(handlerTypes);
        var mediator = new Mediator(provider);

        // act
        var result = mediator.SendAsync(new CommonWithNoResponse.Command());

        // assert
        var ex = Assert.Throws<MediatorExecutionException>([ExcludeFromCodeCoverage] () => 
                    mediator.Send(new CommonWithNoResponse.Command()));
        Assert.AreEqual(typeof(ArgumentException), ex.InnerException!.GetType());
        Assert.AreEqual("Send", ex.Operation);
    }
}

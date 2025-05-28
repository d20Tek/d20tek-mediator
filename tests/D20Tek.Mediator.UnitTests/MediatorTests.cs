using D20Tek.Mediator.UnitTests.Commands;
using D20Tek.Mediator.UnitTests.Mocks;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace D20Tek.Mediator.UnitTests;

[TestClass]
public sealed class MediatorTests
{
    [TestMethod]
    public async Task SendAsync_WithCommandResponse_ReturnsSuccess()
    {
        // arrange
        var provider = ServiceProviderFactory.CreateWith(
                        typeof(ICommandHandlerAsync<AsyncWithResponse.Command, AsyncWithResponse.Response>),
                        typeof(AsyncWithResponse.Handler));
        var mediator = new Mediator(provider);

        // act
        var result = await mediator.SendAsync(new AsyncWithResponse.Command(true), CancellationToken.None);

        // assert
        Assert.AreEqual("success", result.Value);
    }

    [TestMethod]
    public async Task SendAsync_WithCommandResponse_ReturnsFailed()
    {
        // arrange
        var provider = ServiceProviderFactory.CreateWith(
                        typeof(ICommandHandlerAsync<AsyncWithResponse.Command, AsyncWithResponse.Response>),
                        typeof(AsyncWithResponse.Handler));
        var mediator = new Mediator(provider);

        // act
        var result = await mediator.SendAsync(new AsyncWithResponse.Command(false), CancellationToken.None);

        // assert
        Assert.AreEqual("failed", result.Value);
    }

    [TestMethod]
    public async Task SendAsync_WithCommandNoResponse_ReturnsTask()
    {
        // arrange
        var provider = ServiceProviderFactory.CreateWith(
                        typeof(ICommandHandlerAsync<AsyncWithNoResponse.Command>),
                        typeof(AsyncWithNoResponse.Handler));
        var mediator = new Mediator(provider);

        // act
        var task = mediator.SendAsync(new AsyncWithNoResponse.Command(), CancellationToken.None);
        await task;

        // assert
        Assert.IsNotNull(task);
        Assert.IsTrue(task.IsCompleted);
    }

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
        var ex = Assert.Throws<TargetInvocationException>([ExcludeFromCodeCoverage] () => 
                    mediator.Send(new CommonWithNoResponse.Command()));
        Assert.AreEqual(typeof(ArgumentException), ex.InnerException!.GetType());
    }


}

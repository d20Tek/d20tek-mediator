namespace D20Tek.Mediator.UnitTests;

[TestClass]
public class MediatorAsyncTests
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
}

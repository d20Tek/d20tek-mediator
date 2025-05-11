using D20Tek.Mediator.UnitTests.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator.UnitTests;

[TestClass]
public sealed class MediatorTests
{
    [TestMethod]
    public async Task SendAsync_WithCommandResponse_ReturnsSuccess()
    {
        // arrange
        var provider = CreateServiceProvideWith(
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
        var provider = CreateServiceProvideWith(
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
        var provider = CreateServiceProvideWith(
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

    private IServiceProvider CreateServiceProvideWith(Type service, Type implementation)
    {
        var services = new ServiceCollection();
        services.AddScoped(service, implementation);

        return services.BuildServiceProvider();
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator.UnitTests;

[TestClass]
public sealed class DependencyInjectionTests
{
    private readonly ServiceCollection services = new();

    [TestMethod]
    public void AddMediator_WithNoAssembly_OnlyAddsRequiredServices()
    {
        // arrange

        // act
        services.AddMediator();

        // assert
        Assert.AreEqual(1, services.Count);
        Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IMediator)));
    }

    [TestMethod]
    public void AddMediator_WithType_AddsAssemblyServices()
    {
        // arrange

        // act
        services.AddMediatorFor<AsyncWithResponse>();

        // assert
        Assert.AreEqual(11, services.Count);
        Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IMediator)));
        Assert.IsTrue(services.Any(d => 
            d.ServiceType == typeof(ICommandHandlerAsync<AsyncWithResponse.Command, AsyncWithResponse.Response>)));
        Assert.IsTrue(services.Any(
            d => d.ServiceType == typeof(ICommandHandlerAsync<AsyncWithNoResponse.Command>)));
        Assert.IsTrue(services.Any(
            d => d.ServiceType == typeof(ICommandHandler<SyncWithResponse.Command, SyncWithResponse.Response>)));
        Assert.IsTrue(services.Any(
            d => d.ServiceType == typeof(ICommandHandler<SyncWithNoResponse.Command>)));
        Assert.IsTrue(services.Any(
            d => d.ServiceType == typeof(INotificationHandlerAsync<AsyncSubscriber.TestEvent>)));
        Assert.IsTrue(services.Any(d => d.ServiceType == typeof(INotificationHandler<SyncSubscriber.TestEvent>)));
    }

    [TestMethod]
    public void AddMediator_WithAssembly_AddsRequiredServices()
    {
        // arrange

        // act
        services.AddMediator(typeof(AssemblyRegistrar).Assembly, ServiceLifetime.Singleton);

        // assert
        Assert.AreEqual(1, services.Count);
        Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IMediator)));
    }
}

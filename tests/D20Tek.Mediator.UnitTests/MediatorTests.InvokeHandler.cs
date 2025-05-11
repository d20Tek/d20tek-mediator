using D20Tek.Mediator.UnitTests.Commands;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Mediator.UnitTests;

[TestClass]

public sealed class MediatorInvokeHandlerTests
{
    [TestMethod]
    public void InvokeHandler_WithIncorrectCommandType_ThrowsException()
    {
        // arrange
        var handler = new SyncWithNoResponse.Handler();
        var handlerType = typeof(SyncWithNoResponse.Handler);
        var command = new SyncWithNoResponse.Command();

        // act-assert
        Assert.ThrowsExactly<InvalidOperationException>(
            [ExcludeFromCodeCoverage] () =>
                Mediator.InvokeHandler(handler, handlerType, "HandleAsync", [command], true));
    }

    [TestMethod]
    public void InvokeHandler_WithNullReturnWhenExcepted_ThrowsException()
    {
        // arrange
        var handler = new SyncWithNoResponse.Handler();
        var handlerType = typeof(SyncWithNoResponse.Handler);
        var command = new SyncWithNoResponse.Command();

        // act-assert
        Assert.ThrowsExactly<InvalidOperationException>(
            [ExcludeFromCodeCoverage] () => Mediator.InvokeHandler(handler, handlerType, "Handle", [command], false));
    }
}

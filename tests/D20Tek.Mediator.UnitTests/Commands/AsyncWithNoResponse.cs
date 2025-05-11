using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Mediator.UnitTests.Commands;

internal class AsyncWithNoResponse
{
    [ExcludeFromCodeCoverage]
    public record Command : ICommand;

    public sealed class Handler : ICommandHandlerAsync<Command>
    {
        public Task HandleAsync(Command command, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}

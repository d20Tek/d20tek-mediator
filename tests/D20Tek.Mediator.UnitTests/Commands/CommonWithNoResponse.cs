namespace D20Tek.Mediator.UnitTests.Commands;

[ExcludeFromCodeCoverage]
internal class CommonWithNoResponse
{
    public record Command : ICommand;

    public sealed class Handler : ICommandHandler<Command>
    {
        public void Handle(Command command) { }
    }

    public sealed class HandlerAsync : ICommandHandlerAsync<Command>
    {
        public Task HandleAsync(Command command, CancellationToken token) => Task.CompletedTask;
    }
}

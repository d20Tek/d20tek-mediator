namespace D20Tek.Mediator.UnitTests.Commands;

internal class SyncWithNoResponse
{
    [ExcludeFromCodeCoverage]
    public record Command : ICommand;

    public sealed class Handler : ICommandHandler<Command>
    {
        public void Handle(Command command) { }
    }
}

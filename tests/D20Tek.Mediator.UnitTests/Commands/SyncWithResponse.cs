namespace D20Tek.Mediator.UnitTests.Commands;

internal class SyncWithResponse
{
    public record Response(string Value);

    public record Command(bool Input) : ICommand<Response>;

    public sealed class Handler : ICommandHandler<Command, Response>
    {
        public Response Handle(Command command) =>
            new(command.Input ? "success" : "failed");
    }
}

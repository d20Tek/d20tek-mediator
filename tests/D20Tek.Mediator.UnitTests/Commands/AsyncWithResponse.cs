namespace D20Tek.Mediator.UnitTests.Commands;

internal class AsyncWithResponse
{
    public record Response(string Value);

    public record Command(bool Input) : ICommand<Response>;

    public sealed class Handler : ICommandHandlerAsync<Command, Response>
    {
        public Task<Response> HandleAsync(Command command, CancellationToken cancellationToken) =>
            Task.FromResult(new Response(command.Input ? "success" : "failed"));
    }
}

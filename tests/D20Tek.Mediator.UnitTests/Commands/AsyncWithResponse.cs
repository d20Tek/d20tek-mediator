using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Mediator.UnitTests.Commands;

internal class AsyncWithResponse
{
    [ExcludeFromCodeCoverage]
    public record Response(string Value);

    [ExcludeFromCodeCoverage]
    public record Command(bool Input) : ICommand<Response>;

    public sealed class Handler : ICommandHandlerAsync<Command, Response>
    {
        public Task<Response> HandleAsync(Command command, CancellationToken cancellationToken) =>
            Task.FromResult(new Response(command.Input ? "success" : "failed"));
    }
}

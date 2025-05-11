using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Mediator.UnitTests.Commands;

internal class SyncWithResponse
{
    [ExcludeFromCodeCoverage]
    public record Response(string Value);

    [ExcludeFromCodeCoverage]
    public record Command(bool Input) : ICommand<Response>;

    public sealed class Handler : ICommandHandler<Command, Response>
    {
        public Response Handle(Command command) =>
            new(command.Input ? "success" : "failed");
    }
}

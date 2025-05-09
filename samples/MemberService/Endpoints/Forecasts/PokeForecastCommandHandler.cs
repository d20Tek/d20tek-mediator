using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal class PokeForecastCommandHandler : ICommandHandlerAsync<PokeCommand>
{
    public Task HandleAsync(PokeCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine("PokeForecastCommandHandler.HandleAsync was called.");
        return Task.CompletedTask;
    }
}

namespace MemberService.Endpoints.Forecasts;

internal sealed class PokeForecastCommandHandlerAsync : ICommandHandlerAsync<PokeCommand>
{
    public Task HandleAsync(PokeCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine("PokeForecastCommandHandler.HandleAsync was called.");
        return Task.CompletedTask;
    }
}

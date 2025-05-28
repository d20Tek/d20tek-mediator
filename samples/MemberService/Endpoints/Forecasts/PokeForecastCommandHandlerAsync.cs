namespace MemberService.Endpoints.Forecasts;

internal sealed class PokeForecastCommandHandlerAsync : ICommandHandlerAsync<PokeAsyncCommand>
{
    public Task HandleAsync(PokeAsyncCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine("PokeForecastCommandHandlerAsync.HandleAsync was called.");
        return Task.CompletedTask;
    }
}

using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal class PokeForecastRequestHandler : IRequestHandler<PokeRequest>
{
    public Task HandleAsync(PokeRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine("PokeForecastRequestHandler.HandleAsync was called.");
        return Task.CompletedTask;
    }
}

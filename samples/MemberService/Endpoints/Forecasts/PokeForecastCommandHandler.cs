namespace MemberService.Endpoints.Forecasts;

internal sealed class PokeForecastCommandHandler : ICommandHandler<PokeCommand>
{
    public void Handle(PokeCommand command) =>
        Console.WriteLine("PokeForecastCommandHandler.Handle was called.");
}

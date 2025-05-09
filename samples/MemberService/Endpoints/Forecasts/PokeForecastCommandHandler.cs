using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal class PokeForecastCommandHandler : ICommandHandler<PokeCommand>
{
    public void Handle(PokeCommand command) =>
        Console.WriteLine("PokeForecastCommandHandler.Handle was called.");
}

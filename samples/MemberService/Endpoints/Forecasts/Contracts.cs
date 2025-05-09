using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal record WeatherForecastCommand : ICommand<WeatherForecast[]>;

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record PokeCommand : ICommand;

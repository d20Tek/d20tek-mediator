namespace MemberService.Endpoints.Forecasts;

internal sealed record WeatherForecastCommand : ICommand<WeatherForecast[]>;

internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal sealed record PokeCommand : ICommand;

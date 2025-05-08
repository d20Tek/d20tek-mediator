namespace MemberService.Endpoints.Forecasts;

internal static class WeatherForecastEndpoints
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public static void MapForecastEndpoints(this IEndpointRouteBuilder routes) =>
        routes.MapGet("api/v1/weatherforecast", static () =>
            Enumerable.Range(1, 5)
                      .Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                )).ToArray()
        ).WithName("GetWeatherForecast");
}

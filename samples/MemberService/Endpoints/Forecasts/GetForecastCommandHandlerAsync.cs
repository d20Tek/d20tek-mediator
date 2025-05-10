namespace MemberService.Endpoints.Forecasts;

internal sealed class GetForecastCommandHandlerAsync : ICommandHandlerAsync<WeatherForecastCommand, WeatherForecast[]>
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public Task<WeatherForecast[]> HandleAsync(WeatherForecastCommand command, CancellationToken cancellationToken) =>
        Task.FromResult(Enumerable.Range(1, 5)
                                  .Select(index =>
                                      new WeatherForecast
                                      (
                                          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                          Random.Shared.Next(-20, 55),
                                          _summaries[Random.Shared.Next(_summaries.Length)]
                                      )).ToArray());
}

using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal class GetForecastRequestHandler : IRequestHandler<WeatherForecastRequest, WeatherForecast[]>
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public Task<WeatherForecast[]> HandleAsync(WeatherForecastRequest request, CancellationToken cancellationToken) =>
        Task.FromResult(Enumerable.Range(1, 5)
                  .Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                )).ToArray());
}

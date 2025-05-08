using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal static class WeatherForecastEndpoints
{
    public static void MapForecastEndpoints(this IEndpointRouteBuilder routes) =>
        routes.MapGet("api/v1/weatherforecast", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            return await mediator.SendAsync(new WeatherForecastRequest(), cancellationToken);
        }).WithName("GetWeatherForecast");
}

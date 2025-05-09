using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal static class WeatherForecastEndpoints
{
    public static void MapForecastEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("api/v1/weatherforecast", async (IMediator mediator, CancellationToken cancellationToken) =>
            await mediator.SendAsync(new WeatherForecastCommand(), cancellationToken)
        ).WithName("GetWeatherForecast");

        routes.MapGet("api/v1/weatherforecast/poke", async (IMediator mediator, CancellationToken cancellationToken) =>
            await mediator.SendAsync(new PokeCommand(), cancellationToken)
        ).WithName("PokeWeatherForecast");
    }
}

using D20Tek.Mediator;

namespace MemberService.Endpoints.Forecasts;

internal static class WeatherForecastEndpoints
{
    public static void MapForecastEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("api/v1/async/weatherforecast",
            async (IMediator mediator, CancellationToken cancellationToken) =>
                await mediator.SendAsync(new WeatherForecastCommand(), cancellationToken))
              .Produces<WeatherForecast[]>()
              .WithName("GetWeatherForecastAsync")
              .WithTags("Weather Service");

        routes.MapGet("api/v1/async/weatherforecast/poke",
            async (IMediator mediator, CancellationToken cancellationToken) =>
                await mediator.SendAsync(new PokeCommand(), cancellationToken))
              .WithName("PokeWeatherForecastAsync")
              .WithTags("Weather Service");

        routes.MapGet("api/v1/weatherforecast", (IMediator mediator) => mediator.Send(new WeatherForecastCommand()))
              .Produces<WeatherForecast[]>()
              .WithName("GetWeatherForecast")
              .WithTags("Weather Service");

        routes.MapGet("api/v1/weatherforecast/poke", (IMediator mediator) => mediator.Send(new PokeCommand()))
              .WithName("PokeWeatherForecast")
              .WithTags("Weather Service");
    }
}

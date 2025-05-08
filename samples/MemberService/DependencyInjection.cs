using D20Tek.Mediator;
using MemberService.Endpoints.Forecasts;

namespace MemberService;

internal static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddMediator(typeof(DependencyInjection).Assembly)
                        .AddScoped<IRequestHandlerAsync<WeatherForecastRequest, WeatherForecast[]>, GetForecastRequestHandler>()
                        .AddScoped<IRequestHandlerAsync<PokeRequest>, PokeForecastRequestHandler>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        // Map endpoints
        app.MapForecastEndpoints();

        return app;
    }
}

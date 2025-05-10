using MemberService.Endpoints.Forecasts;
using MemberService.Endpoints.Members;
using MemberService.Persistence;
using Scalar.AspNetCore;

namespace MemberService;

internal static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddPersistence()
                        .AddMediator(typeof(DependencyInjection).Assembly);
                        //.AddScoped<ICommandHandlerAsync<WeatherForecastCommand, WeatherForecast[]>, GetForecastCommandHandlerAsync>()
                        //.AddScoped<ICommandHandlerAsync<PokeCommand>, PokeForecastCommandHandlerAsync>();

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
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        // Map endpoints
        app.MapMemberEndpoints()
           .MapForecastEndpoints();

        return app;
    }
}

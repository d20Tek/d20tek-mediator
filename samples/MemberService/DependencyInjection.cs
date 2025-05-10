using D20Tek.LowDb;
using D20Tek.Mediator;
using MemberService.Endpoints.Forecasts;
using MemberService.Endpoints.Members;
using Scalar.AspNetCore;

namespace MemberService;

internal static class DependencyInjection
{
    public const string _databaseFile = "member-data.json";

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddLowDbAsync<MemberDataStore>(_databaseFile);
        builder.Services.AddMediator(typeof(DependencyInjection).Assembly);
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

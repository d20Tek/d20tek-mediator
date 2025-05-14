using D20Tek.Mediator;
using Microsoft.AspNetCore.Mvc;
using SampleApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddMediator();
//builder.Services.AddScoped<ICommandHandler<GetForecasts.Command, ForecastResponse[]>, GetForecasts.Handler>();

builder.Services.AddMediatorFor<Program>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", ([FromServices] IMediator mediator) =>
     mediator.Send(new GetForecasts.Command()))
   .WithName("GetWeatherForecast");

app.MapGet("/weatherforecast2", ([FromServices] ICommandHandler<GetForecasts.Command, ForecastResponse[]> handler) =>
     handler.Handle(new GetForecasts.Command()))
   .WithName("GetWeatherForecast2");

app.Run();

# d20tek-mediator

## Introduction
Welcome to D20Tek.Mediator. This library is a minimal implementation of the Mediator and Command pattern that can be used in any .NET project, whether it is a console application, WebApi, or Blazor application. This code is inspired by the Mediatr package. With Mediatr moving to a commercial license, I built a simplified set of use cases for my own projects as D20Tek.Mediator.

The current version of this package supports the Mediator to send commands (both sync and async) to registered CommandHandlers. Commands are routed to the appropriate handler using the ICommand and ICommandHandlerAsync (or ICommandHandler) interfaces.
Along with commands, the Mediator also supports multi-cast notifications (both sync and async) to registered NotificationHandlers. Notifications are routed to all registered handlers for the same INotification and INotificationHandlerAsync (or INotificationHAndler) interfaces -- regardless of how many handlers are defined.

There are also dependency injection registration functions that will register the Mediator services and all of the command handlers in an assembly (optionally). If you wish to manually register your command handlers, that is also supported.

In the future, we may support more advanced funtionality, like command pipelines to reduce duplicative code. Is that a feature that you require in your projects? Please leave a comment, and we may add that work to our backlog sooner.

## Installation
This library is a NuGet package so it is easy to add to your project. To install the package into your solution, you can use the NuGet Package Manager. In PM, please use the following command:

```cmd
PM > Install-Package D20Tek.Mediator -Version 0.9.7
```

To install in the Visual Studio UI, go to the Tools menu > "Manage NuGet Packages". Then search for D20Tek.Mediator, and install whichever packages you require from there.

## Usage
Once you've installed the NuGet package, you can start using it in your .NET projects. Within your WebApi project, you can start by registering the services with your DI container.

```csharp
    builder.Services.AddMediator(typeof(Program).Assembly);
```

Then, you need to define your command and handler.

```csharp
internal sealed record WeatherForecastCommand : ICommand<WeatherForecast[]>;

internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

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
```

Finally, use the command in your WebApi endpoint (this example uses MinimalApis, but it also works with Controllers).

```csharp
    app.MapGet("api/v1/async/weatherforecast", async (IMediator mediator, CancellationToken cancellationToken) =>
                await mediator.SendAsync(new WeatherForecastCommand(), cancellationToken))
            .Produces<WeatherForecast[]>()
            .WithName("GetWeatherForecastAsync")
            .WithTags("Weather Service");
```

Note that the endpoint code gets the IMediator service from dependency injection (registered above), creates a new instance of the command, and calls SendAsync to send the message asynchronously. Mediator finds the handler that matches your command and forwards the request by calling the HandleAsync method in that CommandHandler. Everything just works correctly if you call AddMediator with the assembly that contains your commands and handlers.

## Samples
For more detailed examples on how to use D20Tek.Mediator, please review the following samples:

* [MemberService](samples/MemberService) - Minimal WebApi project that implements CRUD operations for a members database. It uses asynchronous endpoint definitions, commands, and command handlers to show how D20Tek.Mediator can be used for this type of service.
* [SampleApi](samples/SampleApi) - The simplest of Minimal WepApi projects with only variations of retrieving forecast endpoints.
* [TipCalc.Cli](samples/TipCalc.Cli) - This project shows how you can use D20Tek.Mediator in a console application. It creates a generic host to support dependency injection. Then uses command and handlers in a particular workflow to implement a tip calculator.

## Feedback
If you use this library and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository. I'm still in the process of building the library and samples, so any suggestions that would make it more useable are welcome.

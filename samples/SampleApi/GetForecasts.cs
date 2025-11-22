using D20Tek.Mediator;

namespace SampleApi;

public sealed class GetForecasts
{
    public record Command : ICommand<ForecastResponse[]>;

    public class Handler : ICommandHandler<Command, ForecastResponse[]>
    {
        private static readonly string[] _summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public ForecastResponse[] Handle(Command command) =>
        [.. Enumerable.Range(1, 5).Select(index =>
            new ForecastResponse
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            ))
        ];
    }
}

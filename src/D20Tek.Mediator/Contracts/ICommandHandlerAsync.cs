namespace D20Tek.Mediator;

public interface ICommandHandlerAsync<in TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface ICommandHandlerAsync<in TRequest>
    where TRequest : ICommand
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

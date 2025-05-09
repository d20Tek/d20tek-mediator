namespace D20Tek.Mediator;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);

    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand;

    TResponse Send<TResponse>(ICommand<TResponse> command);

    void Send<TCommand>(TCommand command)
        where TCommand : ICommand;
}

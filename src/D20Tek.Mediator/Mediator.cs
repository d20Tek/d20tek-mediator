namespace D20Tek.Mediator;

public partial class Mediator : IMediator
{
    private const string _asyncFunc = "HandleAsync";
    private const string _syncFunc = "Handle";
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TResponse> SendAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        var handler = GetHandler(typeof(ICommandHandlerAsync<,>), command, typeof(TResponse));
        return (Task<TResponse>)InvokeHandler(
            handler.Instance, handler.Type, _asyncFunc, [command, cancellationToken])!;
    }

    public Task SendAsync<TCommand>(
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        var handler = GetHandler(typeof(ICommandHandlerAsync<>), command);
        return (Task)InvokeHandler(handler.Instance, handler.Type, _asyncFunc, [command, cancellationToken])!;
    }

    public TResponse Send<TResponse>(ICommand<TResponse> command)
    {
        var handler = GetHandler(typeof(ICommandHandler<,>), command, typeof(TResponse));
        return (TResponse)InvokeHandler(handler.Instance, handler.Type, _syncFunc, [command])!;
    }

    public void Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handler = GetHandler(typeof(ICommandHandler<>), command);
        InvokeHandler(handler.Instance, handler.Type, _syncFunc, [command], true);
    }

    public Task NotifyAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        var (handlers, type) = GetMultipleHandlers(typeof(INotificationHandlerAsync<>), notification);
        var tasks = handlers.Select(h => (Task)InvokeHandler(h, type, _asyncFunc, [notification, cancellationToken])!)
                            .ToArray();

        return Task.WhenAll(tasks);
    }

    public void Notify<TNotification>(TNotification notification) where TNotification : INotification
    {
        var (handlers, type) = GetMultipleHandlers(typeof(INotificationHandler<>), notification);
        foreach (var handler in handlers)
        {
            InvokeHandler(handler, type, _syncFunc, [notification], true);
        }
    }
}

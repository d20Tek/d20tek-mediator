using D20Tek.Functional;
using D20Tek.Mediator.Wrappers;
using System.Collections.Concurrent;

namespace D20Tek.Mediator;

public partial class Mediator : IMediator
{
    private const string _asyncFunc = "HandleAsync";
    private const string _syncFunc = "Handle";
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();

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
        var (handler, _) = GetHandler(typeof(ICommandHandlerAsync<>), command);
        return CommandHandlerAsyncWrapper.Create(handler, typeof(TCommand))
                                         .HandleAsync(command, cancellationToken);
    }

    public TResponse Send<TResponse>(ICommand<TResponse> command)
    {
        var (handler, _) = GetHandler(typeof(ICommandHandler<,>), command, typeof(TResponse));
        return CommandResponseHandlerWrapper<TResponse>.Create(handler, command.GetType())
                                                       .Handle(command);
    }

    public void Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        var (handler, _) = GetHandler(typeof(ICommandHandler<>), command);
        CommandHandlerWrapper<TCommand>.Create(handler, typeof(TCommand))
                                       .Handle(command);
    }

    public Task NotifyAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        var (handlers, handlerType) = GetNotificationHandlers(typeof(INotificationHandlerAsync<>), notification);
        var tasks = handlers.Select(h => NotificationHandlerAsyncWrapper.Create(h, typeof(TNotification))
                                                                        .HandleAsync(notification, cancellationToken))
                            .ToArray();

        return Task.WhenAll(tasks);
    }

    public void Notify<TNotification>(TNotification notification) where TNotification : INotification
    {
        var (handlers, handlerType) = GetNotificationHandlers(typeof(INotificationHandler<>), notification);
        handlers.ForEach(h => NotificationHandlerWrapper.Create(h, typeof(TNotification))
                                                        .Handle(notification));
    }
}

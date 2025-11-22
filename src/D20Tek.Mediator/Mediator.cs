using D20Tek.Mediator.Wrappers;

namespace D20Tek.Mediator;

public partial class Mediator(IServiceProvider provider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();

    private readonly IServiceProvider _provider = provider;

    public Task<TResponse> SendAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default) =>
        TryOperation("SendAsync<TResponse>", () =>
        {
            var handler = GetCommandHandler(typeof(ICommandHandlerAsync<,>), command, typeof(TResponse));
            return CommandResponseHandlerAsyncWrapper<TResponse>.Create(handler, command.GetType())
                                                                .HandleAsync(command, cancellationToken);
        });

    public Task SendAsync<TCommand>(
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand =>
        TryOperation("SendAsync", () =>
        {
            var handler = GetCommandHandler(typeof(ICommandHandlerAsync<>), command);
            return CommandHandlerAsyncWrapper.Create(handler, typeof(TCommand))
                                             .HandleAsync(command, cancellationToken);
        });

    public TResponse Send<TResponse>(ICommand<TResponse> command) =>
        TryOperation("Send<TResponse>", () =>
        {
            var handler = GetCommandHandler(typeof(ICommandHandler<,>), command, typeof(TResponse));
            return CommandResponseHandlerWrapper<TResponse>.Create(handler, command.GetType())
                                                           .Handle(command);
        });

    public void Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        TryOperation("Send", () =>
        {
            var handler = GetCommandHandler(typeof(ICommandHandler<>), command);
            CommandHandlerWrapper<TCommand>.Create(handler, typeof(TCommand))
                                           .Handle(command);
        });
    }

    public Task NotifyAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification =>
        TryOperation("NotifyAsync", () =>
        {
            var handlers = GetNotificationHandlers(typeof(INotificationHandlerAsync<>), notification);
            var tasks = handlers.Select(h => NotificationHandlerAsyncWrapper.Create(h, typeof(TNotification))
                                                .HandleAsync(notification, cancellationToken))
                                .ToArray();

            return Task.WhenAll(tasks);
        });

    public void Notify<TNotification>(TNotification notification) where TNotification : INotification =>
        TryOperation("Notify", () =>
        {
            var handlers = GetNotificationHandlers(typeof(INotificationHandler<>), notification);
            handlers.ForEach(h => NotificationHandlerWrapper.Create(h, typeof(TNotification))
                                                            .Handle(notification));
        });
}

using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator;

internal class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TResponse> SendAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        var handlerType = typeof(ICommandHandlerAsync<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        object handler = _provider.GetRequiredService(handlerType);

        return InvokeHandlerAsync(handler, handlerType, command, cancellationToken);
    }

    public Task SendAsync<TCommand>(
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        var handlerType = typeof(ICommandHandlerAsync<>).MakeGenericType(command.GetType());
        object handler = _provider.GetRequiredService(handlerType);

        return InvokeHandlerAsync(handler, handlerType, command, cancellationToken);
    }

    private static Task<TResponse> InvokeHandlerAsync<TResponse>(
        object handler, Type handlerType, ICommand<TResponse> command, CancellationToken cancellationToken)
    {
        var method = handlerType.GetMethod("HandleAsync") ??
            throw new InvalidOperationException($"Handler for {handlerType.Name} does not contain HandleAsync method");

        var task = method.Invoke(handler, [command, cancellationToken]) ??
            throw new InvalidOperationException($"Handler for {handlerType.Name}.HandleAsync invocation failed.");

        return (Task<TResponse>)task!;
    }

    private static Task InvokeHandlerAsync(
        object handler, Type handlerType, ICommand command, CancellationToken cancellationToken)
    {
        var method = handlerType.GetMethod("HandleAsync") ??
            throw new InvalidOperationException($"Handler for {handlerType.Name} does not contain HandleAsync method");

        var task = method.Invoke(handler, [command, cancellationToken]) ??
            throw new InvalidOperationException($"Handler for {handlerType.Name}.HandleAsync invocation failed.");

        return (Task)task!;
    }
}

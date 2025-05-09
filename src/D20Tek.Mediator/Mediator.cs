using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator;

internal partial class Mediator : IMediator
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

    public void Send<TCommand>(TCommand command)
        where TCommand : ICommand
    {
        var handler = GetHandler(typeof(ICommandHandler<>), command);
        InvokeHandler(handler.Instance, handler.Type, _syncFunc, [command], true);
    }

    private (object Instance, Type Type) GetHandler(Type typeInterface, object command, Type? typeResponse = null)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));
        var handlerType = typeResponse is null ?
                            typeInterface.MakeGenericType(command.GetType()) :
                            typeInterface.MakeGenericType(command.GetType(), typeResponse);
        object handler = _provider.GetRequiredService(handlerType);
        return (handler, handlerType);
    }

    private static object? InvokeHandler(
        object handler, Type handlerType, string methodName, object[] parameters, bool voidExpected = false)
    {
        var method = handlerType.GetMethod(methodName)
            ?? throw new InvalidOperationException(
                $"Handler for {handlerType.Name} does not contain {methodName} method");

        var result = method.Invoke(handler, parameters);
        if (voidExpected is false && result is null)
            throw new InvalidOperationException($"Invocation of {handlerType.Name}.{methodName} returned null");

        return result;
    }
}

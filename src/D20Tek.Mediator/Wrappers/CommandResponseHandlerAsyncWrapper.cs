using D20Tek.Mediator.Common;

namespace D20Tek.Mediator.Wrappers;

internal abstract class CommandResponseHandlerAsyncWrapper<TResponse>
{
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public abstract Task<TResponse> HandleAsync(IBaseCommand command, CancellationToken token);

    public static CommandResponseHandlerAsyncWrapper<TResponse> Create(object handler, Type commandType)
    {
        var wrapperType = WrapperTypeDictionary.GetOrAdd(
            commandType,
            et => typeof(CommandResponseHandlerAsyncWrapper<,>).MakeGenericType(et, typeof(TResponse)));

        return (CommandResponseHandlerAsyncWrapper<TResponse>)Activator.CreateInstance(wrapperType, handler)!;
    }
}

// Generic wrapper that provides strong typing for handler invocation
internal sealed class CommandResponseHandlerAsyncWrapper<TCommand, TResponse> :
    CommandResponseHandlerAsyncWrapper<TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandlerAsync<TCommand, TResponse> _handler;

    public CommandResponseHandlerAsyncWrapper(object handler)
    {
        ArgumentTypeExtension.ThrowIfNotAssignableTo<ICommandHandlerAsync<TCommand, TResponse>>(
            handler.GetType(), nameof(handler));
        _handler = (ICommandHandlerAsync<TCommand, TResponse>)handler;
    }

    public override Task<TResponse> HandleAsync(IBaseCommand command, CancellationToken token) =>
        _handler.HandleAsync((TCommand)command, token);
}

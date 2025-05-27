using System.Collections.Concurrent;

namespace D20Tek.Mediator.Wrappers;

internal abstract class CommandHandlerAsyncWrapper
{
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public abstract Task HandleAsync(IBaseCommand command, CancellationToken token);

    public static CommandHandlerAsyncWrapper Create(object handler, Type commandType)
    {
        var wrapperType = WrapperTypeDictionary.GetOrAdd(
            commandType,
            et => typeof(CommandHandlerAsyncWrapper<>).MakeGenericType(et));

        return (CommandHandlerAsyncWrapper)Activator.CreateInstance(wrapperType, handler)!;
    }
}

// Generic wrapper that provides strong typing for handler invocation
internal sealed class CommandHandlerAsyncWrapper<T> : CommandHandlerAsyncWrapper
    where T : ICommand
{
    private readonly ICommandHandlerAsync<T> _handler;

    public CommandHandlerAsyncWrapper(object handler)
    {
        _handler = (ICommandHandlerAsync<T>)handler;
    }

    public override Task HandleAsync(IBaseCommand command, CancellationToken token) =>
        _handler.HandleAsync((T)command, token);
}

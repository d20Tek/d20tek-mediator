using System.Collections.Concurrent;

namespace D20Tek.Mediator.Wrappers;

internal abstract class CommandResponseHandlerWrapper<TResponse>
{
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public abstract TResponse Handle(IBaseCommand command);

    public static CommandResponseHandlerWrapper<TResponse> Create(object handler, Type commandType)
    {
        var wrapperType = WrapperTypeDictionary.GetOrAdd(
            commandType,
            et => typeof(CommandResponseHandlerWrapper<,>).MakeGenericType(et, typeof(TResponse)));

        return (CommandResponseHandlerWrapper<TResponse>)Activator.CreateInstance(wrapperType, handler)!;
    }
}


internal sealed class CommandResponseHandlerWrapper<T, TResponse>(object handler) : CommandResponseHandlerWrapper<TResponse>
    where T : ICommand<TResponse>
{
    private readonly ICommandHandler<T, TResponse> _handler = (ICommandHandler<T, TResponse>)handler;

    public override TResponse Handle(IBaseCommand command) => _handler.Handle((T)command);
}

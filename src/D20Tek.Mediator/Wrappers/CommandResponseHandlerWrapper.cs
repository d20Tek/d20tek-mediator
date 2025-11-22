using D20Tek.Mediator.Common;

namespace D20Tek.Mediator.Wrappers;

internal abstract class CommandResponseHandlerWrapper<TResponse>
{
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = [];

    public abstract TResponse Handle(IBaseCommand command);

    public static CommandResponseHandlerWrapper<TResponse> Create(object handler, Type commandType)
    {
        var wrapperType = WrapperTypeDictionary.GetOrAdd(
            commandType,
            et => typeof(CommandResponseHandlerWrapper<,>).MakeGenericType(et, typeof(TResponse)));

        return (CommandResponseHandlerWrapper<TResponse>)Activator.CreateInstance(wrapperType, handler)!;
    }
}


internal sealed class CommandResponseHandlerWrapper<T, TResponse> : CommandResponseHandlerWrapper<TResponse>
    where T : ICommand<TResponse>
{
    private readonly ICommandHandler<T, TResponse> _handler;

    public CommandResponseHandlerWrapper(object handler)
    {
        ArgumentTypeExtension.ThrowIfNotAssignableTo<ICommandHandler<T, TResponse>>(
            handler.GetType(), nameof(handler));
        _handler = (ICommandHandler<T, TResponse>)handler;
    }

    public override TResponse Handle(IBaseCommand command) => _handler.Handle((T)command);
}

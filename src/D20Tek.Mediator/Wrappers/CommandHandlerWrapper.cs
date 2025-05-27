using System.Collections.Concurrent;

namespace D20Tek.Mediator.Wrappers;

internal abstract class CommandHandlerWrapper
{
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public abstract void Handle(IBaseCommand command);

    public static CommandHandlerWrapper Create(object handler, Type commandType)
    {
        var wrapperType = WrapperTypeDictionary.GetOrAdd(
            commandType,
            et => typeof(CommandHandlerWrapper<>).MakeGenericType(et));

        return (CommandHandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
    }
}

// Generic wrapper that provides strong typing for handler invocation
internal sealed class CommandHandlerWrapper<T> : CommandHandlerWrapper
    where T : ICommand
{
    private readonly ICommandHandler<T> _handler;
    public CommandHandlerWrapper(object handler)
    {
        _handler = (ICommandHandler<T>)handler;
    }

    public override void Handle(IBaseCommand command) => _handler.Handle((T)command);
}

namespace D20Tek.Mediator;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    TResponse Handle(TCommand command);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    void Handle(TCommand command);
}

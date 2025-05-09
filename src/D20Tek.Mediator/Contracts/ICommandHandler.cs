namespace D20Tek.Mediator;

public interface ICommandHandler<in TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    TResponse Handle(TRequest request);
}

public interface ICommandHandler<in TRequest>
    where TRequest : ICommand
{
    void Handle(TRequest request);
}

namespace D20Tek.Mediator;

public interface ICommand : IBaseCommand { }

public interface ICommand<TResponse> : IBaseCommand { }

public interface IBaseCommand { }

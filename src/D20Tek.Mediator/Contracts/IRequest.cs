namespace D20Tek.Mediator;

public interface IRequest : IBaseRequest { }

public interface IRequest<TResult> : IBaseRequest { }

public interface IBaseRequest { }
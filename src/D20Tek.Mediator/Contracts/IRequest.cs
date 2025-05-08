namespace D20Tek.Mediator;

public interface IRequest : IBaseRequest { }

public interface IRequest<TResponse> : IBaseRequest { }

public interface IBaseRequest { }

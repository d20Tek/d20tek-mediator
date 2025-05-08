namespace D20Tek.Mediator;

internal class Mediator : IMediator
{
    private IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _provider.GetService(handlerType) as IRequestHandler<IRequest<TResponse>, TResponse>
            ?? throw new InvalidOperationException($"No handler registered for {request.GetType().Name}");

        return handler.HandleAsync(request, cancellationToken);
    }

    public Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());

        var handler = _provider.GetService(handlerType) as IRequestHandler<IRequest>
            ?? throw new InvalidOperationException($"No handler registered for {request.GetType().Name}");

        return handler.HandleAsync(request, cancellationToken);
    }
}

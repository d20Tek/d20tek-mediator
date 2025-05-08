using Microsoft.Extensions.DependencyInjection;

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

        var handlerType = typeof(IRequestHandlerAsync<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        object handler = _provider.GetRequiredService(handlerType);

        return InvokeHandlerAsync(handler, handlerType, request, cancellationToken);
    }

    public Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var handlerType = typeof(IRequestHandlerAsync<>).MakeGenericType(request.GetType());
        object handler = _provider.GetRequiredService(handlerType);

        return InvokeHandlerAsync(handler, handlerType, request, cancellationToken);
    }

    private static Task<TResponse> InvokeHandlerAsync<TResponse>(
        object handler, Type handlerType, IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var method = handlerType.GetMethod("HandleAsync") ??
            throw new InvalidOperationException($"Handler for {handlerType.Name} does not contain HandleAsync method");

        var task = method.Invoke(handler, [request, cancellationToken]) ??
            throw new InvalidOperationException($"Handler for {handlerType.Name}.HandleAsync invocation failed.");

        return (Task<TResponse>)task!;
    }

    private static Task InvokeHandlerAsync(
        object handler, Type handlerType, IRequest request, CancellationToken cancellationToken)
    {
        var method = handlerType.GetMethod("HandleAsync") ??
            throw new InvalidOperationException($"Handler for {handlerType.Name} does not contain HandleAsync method");

        var task = method.Invoke(handler, [request, cancellationToken]) ??
            throw new InvalidOperationException($"Handler for {handlerType.Name}.HandleAsync invocation failed.");

        return (Task)task!;
    }
}

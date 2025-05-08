using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Mediator;

internal class Mediator : IMediator
{
    private IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var handler = _provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return handler.HandleAsync(request, cancellationToken);
    }

    public Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var handler = _provider.GetRequiredService<IRequestHandler<TRequest>>();
        return handler.HandleAsync(request, cancellationToken);
    }
}

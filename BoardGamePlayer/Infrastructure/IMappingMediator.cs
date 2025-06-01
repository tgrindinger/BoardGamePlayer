using MediatR;

namespace BoardGamePlayer.Infrastructure;

public interface IMappingMediator<TRequest, TResponse>
{
    Task<TResponse> Send(TRequest request, CancellationToken cancellationToken);
}

public class CustomMappingMediator<TRequest, TResponse, TIntermediateRequest, TIntermediateResponse>(
    IMediator _mediator,
    CustomMapping<TRequest, TIntermediateRequest> _requestMapper,
    CustomMapping<TIntermediateResponse, TResponse> _responseMapper)
    : IMappingMediator<TRequest, TResponse>
    where TIntermediateRequest : IRequest<TIntermediateResponse>
{
    public async Task<TResponse> Send(TRequest request, CancellationToken cancellationToken)
    {
        var intermediateRequest = _requestMapper.Map(request);
        var intermediateResponse = await _mediator.Send(intermediateRequest, cancellationToken);
        return _responseMapper.Map(intermediateResponse);
    }
}

public class CustomMapping<TSrc, TDest>(Func<TSrc, TDest> _mapping)
{
    public TDest Map(TSrc src) => _mapping(src);
}

public static class CustomMappingServiceCollectionExtensions
{
    public static IServiceCollection AddScopedMapping<TSrc, TDest>(
        this IServiceCollection services,
        Func<TSrc, TDest> mapping)
    {
        services.AddScoped(_ => new CustomMapping<TSrc, TDest>(mapping));
        return services;
    }
}

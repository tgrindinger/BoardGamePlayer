using FluentValidation;
using MediatR;

namespace BoardGamePlayer.Infrastructure;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var tasks = validators
            .Select(v => v.ValidateAsync(context, cancellationToken));
        var results = await Task.WhenAll(tasks.ToList());
        var failures = results
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures); // Or return a Result<T> pattern

        return await next(cancellationToken);
    }
}

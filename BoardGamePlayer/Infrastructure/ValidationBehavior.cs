using FluentValidation;
using GreenPipes;
using MassTransit;

namespace BoardGamePlayer.Infrastructure;

public class ValidationBehavior<T>(IValidator<T> _validator)
    : IFilter<ConsumeContext<T>>
    where T : class
{
    public void Probe(ProbeContext context)
    {
        context.CreateScope("validation");
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var validationResult = await _validator.ValidateAsync(context.Message);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        await next.Send(context);
    }
}

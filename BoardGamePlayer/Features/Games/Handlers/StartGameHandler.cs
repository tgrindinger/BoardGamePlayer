using FluentValidation;
using BoardGamePlayer.Infrastructure.Exceptions;
using MediatR;
using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;

namespace BoardGamePlayer.Features.Games.Handlers;

public record StartGameCommand(Guid Id, Guid UserId) : IRequest<StartGameResponse>;
public record StartGameResponse(IEnumerable<Card> Hand);

public class StartGameCommandValidator : AbstractValidator<StartGameCommand>
{
    public StartGameCommandValidator(
        IMediator mediator)
    {
        RuleFor(cmd => cmd).CustomAsync(async (cmd, context, cancellationToken) =>
        {
            try
            {
                await mediator.Send(new GetGameQuery(cmd.Id, cmd.UserId), cancellationToken);
            }
            catch (NotFoundException)
            {
                context.AddFailure($"No game found with Id {cmd.Id} and UserId {cmd.UserId}");
            }
        });
    }
}

public class StartGameHandler(
    CommandDbContext _db)
    : IRequestHandler<StartGameCommand, StartGameResponse>
{
    public async Task<StartGameResponse> Handle(
        StartGameCommand command,
        CancellationToken cancellationToken)
    {
        // todo: fill in game domain with deck, board, hand states
        var startingHand = new Deck().DealStartingHand();
        return await Task.FromResult(new StartGameResponse(startingHand.Cards));
    }
}

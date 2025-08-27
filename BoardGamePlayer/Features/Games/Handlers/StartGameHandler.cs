using BoardGamePlayer.Infrastructure.Exceptions;
using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;
using MassTransit;
using FluentValidation;

namespace BoardGamePlayer.Features.Games.Handlers;

public record StartGameCommand(Guid Id, Guid UserId);
public record StartGameResponse(IEnumerable<Card> Hand);

public class StartGameCommandValidator : AbstractValidator<StartGameCommand>
{
    public StartGameCommandValidator() { }
}

public class StartGameHandler(
    CommandDbContext _db)
    : IConsumer<StartGameCommand>
{
    public async Task Consume(ConsumeContext<StartGameCommand> context)
    {
        if (!_db.Games.Any(game => game.Id == context.Message.Id))
        {
            throw new NotFoundException("Game not found");
        }
        // todo: fill in game domain with deck, board, hand states
        var startingHand = new Deck().DealStartingHand();
        await context.RespondAsync(new StartGameResponse(startingHand.Cards));
    }
}

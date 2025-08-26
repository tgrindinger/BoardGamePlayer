using BoardGamePlayer.Infrastructure.Exceptions;
using MediatR;
using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;

namespace BoardGamePlayer.Features.Games.Handlers;

public record StartGameCommand(Guid Id, Guid UserId) : IRequest<StartGameResponse>;
public record StartGameResponse(IEnumerable<Card> Hand);

public class StartGameHandler(
    CommandDbContext _db)
    : IRequestHandler<StartGameCommand, StartGameResponse>
{
    public async Task<StartGameResponse> Handle(
        StartGameCommand command,
        CancellationToken cancellationToken)
    {
        if (!_db.Games.Any(game => game.Id == command.Id))
        {
            throw new NotFoundException("Game not found");
        }
        // todo: fill in game domain with deck, board, hand states
        var startingHand = new Deck().DealStartingHand();
        return await Task.FromResult(new StartGameResponse(startingHand.Cards));
    }
}

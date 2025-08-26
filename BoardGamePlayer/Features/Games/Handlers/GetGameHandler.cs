using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;
using BoardGamePlayer.Infrastructure.Exceptions;
using MediatR;

namespace BoardGamePlayer.Features.Games.Handlers;

// public contracts
public record GetGameQuery(Guid Id, Guid UserId) : IRequest<GetGameResponse>;
public record GetGameResponse(Guid Id, string Title, GameStatus State);

// handler
public class GetGameHandler(
    QueryDbContext _db)
    : IRequestHandler<GetGameQuery, GetGameResponse>
{
    public async Task<GetGameResponse> Handle(
        GetGameQuery query,
        CancellationToken cancellationToken)
    {
        var game = _db.Games.FirstOrDefault(g => g.Id == query.Id);
        if (game == default(Game) || game.UserId != query.UserId)
        {
            throw new NotFoundException($"The game with Id {query.Id} was not found for user {query.UserId}.");
        }
        return await Task.FromResult(new GetGameResponse(game.Id, game.Title, game.GameStatus));
    }
}

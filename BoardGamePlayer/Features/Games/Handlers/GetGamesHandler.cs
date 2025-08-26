using BoardGamePlayer.Data;
using MediatR;

namespace BoardGamePlayer.Features.Games.Handlers;

public record GetGamesQuery(Guid UserId) : IRequest<GetGamesResponse>;
public record GetGamesResponse(IEnumerable<Guid> GameIds);

public class GetGamesHandler(
    QueryDbContext _db)
    : IRequestHandler<GetGamesQuery, GetGamesResponse>
{
    public async Task<GetGamesResponse> Handle(
        GetGamesQuery query,
        CancellationToken cancellationToken)
    {
        var games = _db.Games.Where(game => game.UserId == query.UserId);
        return await Task.FromResult(new GetGamesResponse(games.Select(g => g.Id)));
    }
}

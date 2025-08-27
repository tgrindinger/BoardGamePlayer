using BoardGamePlayer.Data;
using FluentValidation;
using MassTransit;

namespace BoardGamePlayer.Features.Games.Handlers;

public record GetGamesQuery(Guid UserId);
public record GetGamesResponse(IEnumerable<Guid> GameIds);

public class GetGamesQueryValidator : AbstractValidator<GetGamesQuery>
{
    public GetGamesQueryValidator() { }
}

public class GetGamesHandler(
    QueryDbContext _db)
    : IConsumer<GetGamesQuery>
{
    public async Task Consume(ConsumeContext<GetGamesQuery> context)
    {
        var games = _db.Games.Where(game => game.UserId == context.Message.UserId);
        await context.RespondAsync(new GetGamesResponse(games.Select(g => g.Id)));
    }
}

using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;
using BoardGamePlayer.Infrastructure.Exceptions;
using FluentValidation;
using MassTransit;

namespace BoardGamePlayer.Features.Games.Handlers;

public record GetGameQuery(Guid Id, Guid UserId);
public record GetGameResponse(Guid Id, string Title, GameStatus State);

public class GetGameQueryValidator : AbstractValidator<GetGameQuery>
{
    public GetGameQueryValidator() { }
}

public class GetGameHandler(
    QueryDbContext _db)
    : IConsumer<GetGameQuery>
{
    public async Task Consume(ConsumeContext<GetGameQuery> context)
    {
        var game = _db.Games.FirstOrDefault(g => g.Id == context.Message.Id);
        if (game == default(Game) || game.UserId != context.Message.UserId)
        {
            throw new NotFoundException($"The game with Id {context.Message.Id} was not found for user {context.Message.UserId}.");
        }
        await context.RespondAsync(new GetGameResponse(game.Id, game.Title, game.GameStatus));
    }
}

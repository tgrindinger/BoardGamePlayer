using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;
using BoardGamePlayer.Infrastructure.Exceptions;
using FluentValidation;
using MassTransit;

namespace BoardGamePlayer.Features.Games.Handlers;

public record CreateGameCommand(Guid UserId, string Title);
public record CreateGameResponse(Guid Id, bool IsCreated);

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator()
    {
        RuleFor(cmd => cmd.Title).NotEmpty();
        RuleFor(cmd => cmd.UserId).NotEmpty();
    }
}

public class CreateGameHandler(
    CommandDbContext _db)
    : IConsumer<CreateGameCommand>
{
    public async Task Consume(ConsumeContext<CreateGameCommand> context)
    {
        if (!_db.Users.Any(user => user.Id == context.Message.UserId))
        {
            throw new NotFoundException($"User not found.");
        }
        var existingGame = _db.Games.FirstOrDefault(game => game.Title == context.Message.Title && game.UserId == context.Message.UserId);
        if (existingGame != default(Game))
        {
            await context.RespondAsync(new CreateGameResponse(existingGame.Id, false));
            return;
        }
        var game = new Game { UserId = context.Message.UserId, GameStatus = GameStatus.Created, Title = context.Message.Title };
        var savedGame = _db.Games.Add(game);
        await _db.SaveChangesAsync(context.CancellationToken);
        await context.RespondAsync(new CreateGameResponse(savedGame.Entity.Id, true));
    }
}

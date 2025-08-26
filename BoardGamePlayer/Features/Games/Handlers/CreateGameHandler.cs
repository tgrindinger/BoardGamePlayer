using FluentValidation;
using MediatR;
using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;
using BoardGamePlayer.Features.Users.Handlers;
using BoardGamePlayer.Infrastructure.Exceptions;

namespace BoardGamePlayer.Features.Games.Handlers;

public record CreateGameCommand(Guid UserId, string Title) : IRequest<CreateGameResponse>;
public record CreateGameResponse(Guid Id, bool IsCreated);

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator(IMediator _mediator)
    {
        RuleFor(cmd => cmd.Title).NotEmpty();
        RuleFor(cmd => cmd.UserId).NotEmpty();
        RuleFor(cmd => cmd.UserId).CustomAsync(async (id, context, cancellationToken) =>
        {
            try
            {
                await _mediator.Send(new GetUserQuery(id, null), cancellationToken);
            }
            catch (NotFoundException)
            {
                context.AddFailure($"No user found with Id {id}");
            }
        });
    }
}

public class CreateGameHandler(
    CommandDbContext _db)
    : IRequestHandler<CreateGameCommand, CreateGameResponse>
{
    public async Task<CreateGameResponse> Handle(
        CreateGameCommand command,
        CancellationToken cancellationToken)
    {
        var existingGame = _db.Games.FirstOrDefault(game => game.Title == command.Title && game.UserId == command.UserId);
        if (existingGame != default(Game))
        {
            return new CreateGameResponse(existingGame.Id, false);
        }
        var game = new Game { UserId = command.UserId, GameStatus = GameStatus.Created, Title = command.Title };
        var savedGame = _db.Games.Add(game);
        await _db.SaveChangesAsync(cancellationToken);
        return new CreateGameResponse(savedGame.Entity.Id, true);
    }
}

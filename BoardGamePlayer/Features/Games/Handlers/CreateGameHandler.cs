using FluentValidation;
using BoardGamePlayer.Infrastructure;
using BoardGamePlayer.Infrastructure.Exceptions;
using MediatR;

namespace BoardGamePlayer.Features.Games.Handlers;

// public contracts
public record CreateGameCommand(Guid UserId, string Title) : IRequest<CreateGameResponse>;
public record CreateGameResponse(Guid Id, bool IsCreated);

// internal contracts
public record LookupUserQuery(Guid Id) : IRequest<LookupUserResponse>;
public record LookupUserResponse(Guid Id);

// validators
public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator(IMappingMediator<LookupUserQuery, LookupUserResponse> _userMediator)
    {
        RuleFor(game => game.Title).NotEmpty();
        RuleFor(game => game.UserId).NotEmpty();
        RuleFor(game => game.UserId).CustomAsync(async (id, context, cancellationToken) =>
        {
            try
            {
                await _userMediator.Send(new LookupUserQuery(id), cancellationToken);
            }
            catch (NotFoundException)
            {
                context.AddFailure($"No user found with Id {id}");
            }
        });
    }
}

// handlers
public class CreateGameHandler(
    GameAppDbContext _db)
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

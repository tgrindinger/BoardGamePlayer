using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;
using FluentValidation;
using MediatR;

namespace BoardGamePlayer.Features.Users.Handlers;

public record CreateUserCommand(string Name) : IRequest<CreateUserResponse>;
public record CreateUserResponse(Guid Id, bool IsCreated);

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.Name).NotEmpty();
    }
}

public class CreateUserHandler(
    CommandDbContext _db)
    : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var existingUser = _db.Users.FirstOrDefault(user => user.Name == command.Name);
        if (existingUser != default(User))
        {
            return new CreateUserResponse(existingUser.Id, false);
        }
        var user = new User { Name = command.Name };
        var savedUser = _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);
        return new CreateUserResponse(savedUser.Entity.Id, true);
    }
}

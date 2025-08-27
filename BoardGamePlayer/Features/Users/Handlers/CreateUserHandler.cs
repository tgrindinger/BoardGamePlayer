using BoardGamePlayer.Data;
using BoardGamePlayer.Domain;
using FluentValidation;
using MassTransit;

namespace BoardGamePlayer.Features.Users.Handlers;

public record CreateUserCommand(string Name);
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
    : IConsumer<CreateUserCommand>
{
    public async Task Consume(ConsumeContext<CreateUserCommand> context)
    {
        var existingUser = _db.Users.FirstOrDefault(user => user.Name == context.Message.Name);
        if (existingUser != default(User))
        {
            await context.RespondAsync(new CreateUserResponse(existingUser.Id, false));
            return;
        }
        var user = new User { Name = context.Message.Name };
        var savedUser = _db.Users.Add(user);
        await _db.SaveChangesAsync(context.CancellationToken);
        await context.RespondAsync(new CreateUserResponse(savedUser.Entity.Id, true));
    }
}

using BoardGamePlayer.Data;
using BoardGamePlayer.Infrastructure.Exceptions;
using FluentValidation;
using MassTransit;

namespace BoardGamePlayer.Features.Users.Handlers;

public record GetUserQuery(Guid? Id, string? Name);
public record GetUserResponse(Guid Id, string Name);

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator() { }
}

public class GetUserHandler(
    QueryDbContext _db)
    : IConsumer<GetUserQuery>
{
    public async Task Consume(ConsumeContext<GetUserQuery> context)
    {
        var user = _db.Users.FirstOrDefault(u => u.Id == context.Message.Id || u.Name == context.Message.Name);
        if (user == null)
        {
            throw new NotFoundException($"User with Id {context.Message.Id} or Name {context.Message.Name} was not found.");
        }
        await context.RespondAsync(new GetUserResponse(user.Id, user.Name));
    }
}

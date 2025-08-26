using BoardGamePlayer.Data;
using BoardGamePlayer.Infrastructure.Exceptions;
using MediatR;

namespace BoardGamePlayer.Features.Users.Handlers;

public record GetUserQuery(Guid? Id, string? Name) : IRequest<GetUserResponse>;
public record GetUserResponse(Guid Id, string Name);

public class GetUserHandler(
    QueryDbContext _db)
    : IRequestHandler<GetUserQuery, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(
        GetUserQuery query,
        CancellationToken cancellationToken)
    {
        var user = _db.Users.FirstOrDefault(u => u.Id == query.Id || u.Name == query.Name);
        if (user == null)
        {
            throw new NotFoundException($"User with Id {query.Id} or Name {query.Name} was not found.");
        }
        return await Task.FromResult(new GetUserResponse(user.Id, user.Name));
    }
}

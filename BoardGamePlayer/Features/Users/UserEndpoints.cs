using BoardGamePlayer.Features.Users.Handlers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamePlayer.Features.Users;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");
        group.MapPost("", async (CreateUserCommand cmd, IRequestClient<CreateUserCommand> client) =>
            await client.GetResponse<CreateUserResponse>(cmd));
        group.MapGet("", async ([FromQuery] Guid? id, [FromQuery] string? name, IRequestClient<GetUserQuery> client) =>
            await client.GetResponse<GetUserResponse>(new GetUserQuery(id, name)));
    }
}

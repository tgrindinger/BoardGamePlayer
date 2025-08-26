using BoardGamePlayer.Features.Users.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamePlayer.Features.Users;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");
        group.MapPost("", async (CreateUserCommand cmd, IMediator mediator) =>
            await mediator.Send(cmd));
        group.MapGet("", async ([FromQuery] Guid? id, [FromQuery] string? name, IMediator mediator) =>
            await mediator.Send(new GetUserQuery(id, name)));
    }
}

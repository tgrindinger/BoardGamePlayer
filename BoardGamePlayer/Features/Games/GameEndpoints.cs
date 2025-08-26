using BoardGamePlayer.Features.Games.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamePlayer.Features.Games;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games")
            .WithTags("Games");
        group.MapPost("", async (CreateGameCommand cmd, IMediator mediator) =>
            await mediator.Send(cmd));
        group.MapGet("", async ([FromQuery] Guid? id, [FromQuery] Guid userId, IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetGameQuery(id.Value, userId))));
        group.MapGet("/list", async ([FromQuery] Guid userId, IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetGamesQuery(userId))));
        group.MapPost("/start", async (StartGameCommand cmd, IMediator mediator) =>
            await Task.CompletedTask);
    }
}

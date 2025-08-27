using BoardGamePlayer.Features.Games.Handlers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamePlayer.Features.Games;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games")
            .WithTags("Games");
        group.MapPost("", async (CreateGameCommand cmd, IRequestClient<CreateGameCommand> client) =>
            await client.GetResponse<CreateGameResponse>(cmd));
        group.MapGet("", async ([FromQuery] Guid? id, [FromQuery] Guid userId, IRequestClient<GetGameQuery> client) =>
            await client.GetResponse<GetGameResponse>(new GetGameQuery(id.Value, userId)));
        group.MapGet("/list", async ([FromQuery] Guid userId, IRequestClient<GetGamesQuery> client) =>
            await client.GetResponse<GetGamesResponse>(new GetGamesQuery(userId)));
        group.MapPost("/start", async (StartGameCommand cmd, IRequestClient<StartGameCommand> client) =>
            await client.GetResponse<StartGameResponse>(cmd));
    }
}

using BoardGamePlayer.Features.Users.Handlers;
using MediatR;
using Xunit;

namespace BoardGamePlayer.Features.Games.Handlers;

public class GetGamesHandlerTests(IMediator _mediator)
{
    [Fact]
    public async Task GivenIHaveAGame_WhenIGetTheGames_ThenIGetOneGame()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));
        var title = Guid.NewGuid().ToString();
        var game = await _mediator.Send(new CreateGameCommand(user.Id, title));

        // act
        var response = await _mediator.Send(new GetGamesQuery(user.Id));

        // assert
        Assert.Single(response.GameIds);
        Assert.Equal(game.Id, response.GameIds.First());
    }

    [Fact]
    public async Task GivenIHaveMultipleGame_WhenIGetTheGames_ThenIGetAllTheGames()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));
        var gameCount = 5;
        for (var i = 0; i < gameCount; i++)
        {
            await _mediator.Send(new CreateGameCommand(user.Id, Guid.NewGuid().ToString()));
        }

        // act
        var response = await _mediator.Send(new GetGamesQuery(user.Id));

        // assert
        Assert.Equal(gameCount, response.GameIds.Count());
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIGetTheGames_ThenIGetAnEmptyList()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var response = await _mediator.Send(new GetGamesQuery(user.Id));

        // assert
        Assert.Empty(response.GameIds);
    }
}

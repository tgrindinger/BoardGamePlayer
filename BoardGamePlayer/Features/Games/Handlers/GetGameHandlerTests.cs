using BoardGamePlayer.Features.Users.Handlers;
using BoardGamePlayer.Infrastructure.Exceptions;
using MediatR;
using Xunit;

namespace BoardGamePlayer.Features.Games.Handlers;

public class GetGameHandlerTests(IMediator _mediator)
{
    [Fact]
    public async Task GivenIHaveAGame_WhenIGetTheGame_ThenIGetTheGameData()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));
        var title = Guid.NewGuid().ToString();
        var game = await _mediator.Send(new CreateGameCommand(user.Id, title));

        // act
        var response = await _mediator.Send(new GetGameQuery(game.Id, user.Id));

        // assert
        Assert.Equal(game.Id, response.Id);
        Assert.Equal(title, response.Title);
        Assert.Equal(GameStatus.Created, response.State);
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIGetAGame_ThenIGetAnError()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var action = () => _mediator.Send(new GetGameQuery(Guid.NewGuid(), user.Id));

        // assert
        await Assert.ThrowsAsync<NotFoundException>(action);
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIGetAGameThatExists_ThenIGetAnError()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));
        var otherUser = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));
        var game = await _mediator.Send(new CreateGameCommand(otherUser.Id, "not your game"));

        // act
        var action = () => _mediator.Send(new GetGameQuery(game.Id, user.Id));

        // assert
        await Assert.ThrowsAsync<NotFoundException>(action);
    }
}

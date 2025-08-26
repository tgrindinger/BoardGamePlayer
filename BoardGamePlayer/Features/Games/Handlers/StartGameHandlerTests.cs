using FluentValidation;
using BoardGamePlayer.Features.Users.Handlers;
using MediatR;
using Xunit;

namespace BoardGamePlayer.Features.Games.Handlers;

public class StartGameHandlerTests(IMediator _mediator)
{
    private static string RandomString() => Guid.NewGuid().ToString();

    [Fact]
    public async Task GivenIHaveANewGame_WhenIStartTheGame_ThenIGetTheInitialHand()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(RandomString()));
        var game = await _mediator.Send(new CreateGameCommand(user.Id, RandomString()));
        var cmd = new StartGameCommand(game.Id, user.Id);

        // act
        var response = await _mediator.Send(cmd);

        // assert
        Assert.Equal(7, response.Hand.Count());
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIStartAGame_ThenIGetAValidationError()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(RandomString()));
        var cmd = new StartGameCommand(Guid.NewGuid(), user.Id);

        // act
        var action = async () => await _mediator.Send(cmd);

        // assert
        await Assert.ThrowsAsync<ValidationException>(action);
    }

    [Fact]
    public async Task GivenIAmNotAUser_WhenIStartAGame_ThenIGetAValidationError()
    {
        // arrange
        var cmd = new StartGameCommand(Guid.NewGuid(), Guid.NewGuid());

        // act
        var action = async () => await _mediator.Send(cmd);

        // assert
        await Assert.ThrowsAsync<ValidationException>(action);
    }
}


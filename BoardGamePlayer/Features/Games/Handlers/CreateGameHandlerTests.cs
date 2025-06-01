using FluentValidation;
using BoardGamePlayer.Features.Users.Handlers;
using MediatR;
using Xunit;

namespace BoardGamePlayer.Features.Games.Handlers;

public class CreateGameHandlerTests(IMediator _mediator)
{
    [Fact]
    public async Task GivenICanCreateAGame_WhenICreateAGame_ThenIGetTheGameData()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var response = await _mediator.Send(new CreateGameCommand(user.Id, Guid.NewGuid().ToString()));

        // assert
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.True(response.IsCreated);
    }

    [Fact]
    public async Task GivenICanCreateAGame_WhenICreateADuplicateGame_ThenIGetTheExistingGame()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));
        var title = Guid.NewGuid().ToString();
        var game = await _mediator.Send(new CreateGameCommand(user.Id, title));

        // act
        var response = await _mediator.Send(new CreateGameCommand(user.Id, title));

        // assert
        Assert.Equal(game.Id, response.Id);
        Assert.False(response.IsCreated);
    }

    [Fact]
    public async Task GivenICanCreateAGame_WhenICreateAGameWithoutATitle_ThenIGetAValidationError()
    {
        // arrange
        var user = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var command = () => _mediator.Send(new CreateGameCommand(user.Id, ""));

        // assert
        await Assert.ThrowsAsync<ValidationException>(command);
    }

    [Fact]
    public async Task GivenICannotCreateAGame_WhenICreateAGame_ThenIGetANotFoundError()
    {
        // arrange

        // act
        var command = () => _mediator.Send(new CreateGameCommand(Guid.NewGuid(), Guid.NewGuid().ToString()));

        // assert
        await Assert.ThrowsAsync<ValidationException>(command);
    }
}

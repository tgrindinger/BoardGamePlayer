using BoardGamePlayer.Features.Users.Handlers;
using Xunit;
using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace BoardGamePlayer.Features.Games.Handlers;

public class CreateGameHandlerTests(
    IRequestClient<CreateGameCommand> _createGameClient,
    IRequestClient<CreateUserCommand> _createUserClient)
{
    [Fact]
    public async Task GivenICanCreateAGame_WhenICreateAGame_ThenIGetTheGameData()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var response = await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, Guid.NewGuid().ToString()));

        // assert
        Assert.NotEqual(Guid.Empty, response.Message.Id);
        Assert.True(response.Message.IsCreated);
    }

    [Fact]
    public async Task GivenICanCreateAGame_WhenICreateADuplicateGame_ThenIGetTheExistingGame()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));
        var title = Guid.NewGuid().ToString();
        var game = await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, title));

        // act
        var response = await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, title));

        // assert
        Assert.Equal(game.Message.Id, response.Message.Id);
        Assert.False(response.Message.IsCreated);
    }

    [Fact]
    public async Task GivenICanCreateAGame_WhenICreateAGameWithoutATitle_ThenIGetAValidationError()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var command = () => _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, ""));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(command);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(ValidationException), exceptionName);
    }

    [Fact]
    public async Task GivenICannotCreateAGame_WhenICreateAGame_ThenIGetANotFoundError()
    {
        // arrange

        // act
        var command = () => _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(Guid.NewGuid(), Guid.NewGuid().ToString()));

        // assert
        await Assert.ThrowsAsync<RequestFaultException>(command);
    }
}

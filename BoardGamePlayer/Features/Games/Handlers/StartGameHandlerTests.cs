using BoardGamePlayer.Features.Users.Handlers;
using Xunit;
using BoardGamePlayer.Infrastructure.Exceptions;
using MassTransit;

namespace BoardGamePlayer.Features.Games.Handlers;

public class StartGameHandlerTests(
    IRequestClient<CreateGameCommand> _createGameClient,
    IRequestClient<CreateUserCommand> _createUserClient,
    IRequestClient<StartGameCommand> _startGameClient)
{
    private static string RandomString() => Guid.NewGuid().ToString();

    [Fact]
    public async Task GivenIHaveANewGame_WhenIStartTheGame_ThenIGetTheInitialHand()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(RandomString()));
        var game = await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, RandomString()));
        var cmd = new StartGameCommand(game.Message.Id, user.Message.Id);

        // act
        var response = await _startGameClient.GetResponse<StartGameResponse>(cmd);

        // assert
        Assert.Equal(7, response.Message.Hand.Count());
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIStartAGame_ThenIGetAValidationError()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(RandomString()));
        var cmd = new StartGameCommand(Guid.NewGuid(), user.Message.Id);

        // act
        var action = () => _startGameClient.GetResponse<StartGameResponse>(cmd);

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }

    [Fact]
    public async Task GivenIAmNotAUser_WhenIStartAGame_ThenIGetAValidationError()
    {
        // arrange
        var cmd = new StartGameCommand(Guid.NewGuid(), Guid.NewGuid());

        // act
        var action = () => _startGameClient.GetResponse<StartGameResponse>(cmd);

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }
}


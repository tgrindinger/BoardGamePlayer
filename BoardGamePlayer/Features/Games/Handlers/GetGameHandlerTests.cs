using BoardGamePlayer.Domain;
using BoardGamePlayer.Features.Users.Handlers;
using BoardGamePlayer.Infrastructure.Exceptions;
using MassTransit;
using Xunit;

namespace BoardGamePlayer.Features.Games.Handlers;

public class GetGameHandlerTests(
    IRequestClient<CreateGameCommand> _createGameClient,
    IRequestClient<CreateUserCommand> _createUserClient,
    IRequestClient<GetGameQuery> _getGameClient)
{
    [Fact]
    public async Task GivenIHaveAGame_WhenIGetTheGame_ThenIGetTheGameData()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));
        var title = Guid.NewGuid().ToString();
        var game = await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, title));

        // act
        var response = await _getGameClient.GetResponse<GetGameResponse>(new GetGameQuery(game.Message.Id, user.Message.Id));

        // assert
        Assert.Equal(game.Message.Id, response.Message.Id);
        Assert.Equal(title, response.Message.Title);
        Assert.Equal(GameStatus.Created, response.Message.State);
    }

    [Fact]
    public async Task GivenIAmNotAUser_WhenIGetAGame_ThenIGetAnError()
    {
        // arrange
        var userId = Guid.NewGuid();

        // act
        var action = () => _getGameClient.GetResponse<GetGameResponse>(new GetGameQuery(Guid.NewGuid(), userId));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIGetAGame_ThenIGetAnError()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var action = () => _getGameClient.GetResponse<GetGameResponse>(new GetGameQuery(Guid.NewGuid(), user.Message.Id));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIGetAGameThatExists_ThenIGetAnError()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));
        var otherUser = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));
        var game = await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(otherUser.Message.Id, "not your game"));

        // act
        var action = () => _getGameClient.GetResponse<GetGameResponse>(new GetGameQuery(game.Message.Id, user.Message.Id));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }
}

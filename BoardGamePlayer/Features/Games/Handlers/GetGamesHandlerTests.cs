using BoardGamePlayer.Features.Users.Handlers;
using MassTransit;
using Xunit;

namespace BoardGamePlayer.Features.Games.Handlers;

public class GetGamesHandlerTests(
    IRequestClient<CreateGameCommand> _createGameClient,
    IRequestClient<CreateUserCommand> _createUserClient,
    IRequestClient<GetGamesQuery> _getGamesClient)
{
    [Fact]
    public async Task GivenIHaveAGame_WhenIGetTheGames_ThenIGetOneGame()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));
        var title = Guid.NewGuid().ToString();
        var game = await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, title));

        // act
        var response = await _getGamesClient.GetResponse<GetGamesResponse>(new GetGamesQuery(user.Message.Id));

        // assert
        Assert.Single(response.Message.GameIds);
        Assert.Equal(game.Message.Id, response.Message.GameIds.First());
    }

    [Fact]
    public async Task GivenIHaveMultipleGame_WhenIGetTheGames_ThenIGetAllTheGames()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));
        var gameCount = 5;
        for (var i = 0; i < gameCount; i++)
        {
            await _createGameClient.GetResponse<CreateGameResponse>(new CreateGameCommand(user.Message.Id, Guid.NewGuid().ToString()));
        }

        // act
        var response = await _getGamesClient.GetResponse<GetGamesResponse>(new GetGamesQuery(user.Message.Id));

        // assert
        Assert.Equal(gameCount, response.Message.GameIds.Count());
    }

    [Fact]
    public async Task GivenIHaveNoGames_WhenIGetTheGames_ThenIGetAnEmptyList()
    {
        // arrange
        var user = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));

        // act
        var response = await _getGamesClient.GetResponse<GetGamesResponse>(new GetGamesQuery(user.Message.Id));

        // assert
        Assert.Empty(response.Message.GameIds);
    }
}

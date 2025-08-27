using BoardGamePlayer.Infrastructure.Exceptions;
using MassTransit;
using Xunit;

namespace BoardGamePlayer.Features.Users.Handlers;

public class GetUserHandlerTests(
    IRequestClient<CreateUserCommand> _createUserClient,
    IRequestClient<GetUserQuery> _getUserClient)
{
    [Fact]
    public async Task GivenIHaveAUser_WhenIGetTheUserById_ThenIGetTheUser()
    {
        // arrange
        var name = Guid.NewGuid().ToString();
        var savedUser = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(name));

        // act
        var response = await _getUserClient.GetResponse<GetUserResponse>(new GetUserQuery(savedUser.Message.Id, ""));

        // assert
        Assert.Equal(savedUser.Message.Id, response.Message.Id);
        Assert.Equal(name, response.Message.Name);
    }

    [Fact]
    public async Task GivenIHaveAUser_WhenIGetTheUserByName_ThenIGetTheUser()
    {
        // arrange
        var name = Guid.NewGuid().ToString();
        var savedUser = await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(name));

        // act
        var response = await _getUserClient.GetResponse<GetUserResponse>(new GetUserQuery(default(Guid), name));

        // assert
        Assert.Equal(savedUser.Message.Id, response.Message.Id);
        Assert.Equal(name, response.Message.Name);
    }

    [Fact]
    public async Task GivenIHaveNoUsers_WhenIGetAUserByName_ThenIGetANotFoundException()
    {
        // arrange
        var name = Guid.NewGuid().ToString();

        // act
        var action = () => _getUserClient.GetResponse<GetUserResponse>(new GetUserQuery(default(Guid), name));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }

    [Fact]
    public async Task GivenIHaveNoUsers_WhenIGetAUserById_ThenIGetANotFoundException()
    {
        // arrange
        var id = Guid.NewGuid();

        // act
        var action = () => _getUserClient.GetResponse<GetUserResponse>(new GetUserQuery(id, ""));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }

    [Fact]
    public async Task GivenIHaveAUser_WhenIGetAUserByWrongId_ThenIGetANotFoundException()
    {
        // arrange
        var invalidId = Guid.NewGuid();
        var name = Guid.NewGuid().ToString();
        await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(name));

        // act
        var action = () => _getUserClient.GetResponse<GetUserResponse>(new GetUserQuery(invalidId, ""));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }

    [Fact]
    public async Task GivenIHaveAUser_WhenIGetAUserByWrongName_ThenIGetANotFoundException()
    {
        // arrange
        var invalidName = Guid.NewGuid().ToString();
        var name = Guid.NewGuid().ToString();
        await _createUserClient.GetResponse<CreateUserResponse>(new CreateUserCommand(name));

        // act
        var action = () => _getUserClient.GetResponse<GetUserResponse>(new GetUserQuery(default(Guid), invalidName));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(action);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(NotFoundException), exceptionName);
    }
}

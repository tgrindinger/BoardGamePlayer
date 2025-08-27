using FluentValidation;
using MassTransit;
using Xunit;

namespace BoardGamePlayer.Features.Users.Handlers;

public class CreateUserHandlerTests(IRequestClient<CreateUserCommand> _client)
{
    [Fact]
    public async Task GivenICanCreateAUser_WhenICreateAUser_ThenIGetTheirId()
    {
        // arrange

        // act
        var response = await _client.GetResponse<CreateUserResponse>(new CreateUserCommand(Guid.NewGuid().ToString()));

        // assert
        Assert.True(response.Message.Id.ToString().Length > 0);
        Assert.True(response.Message.IsCreated);
    }

    [Fact]
    public async Task GivenICanCreateAUser_WhenICreateAUserWithNoName_ThenIGetAValidationError()
    {
        // arrange

        // act
        var command = async () => await _client.GetResponse<CreateUserResponse>(new CreateUserCommand(""));

        // assert
        var exception = await Assert.ThrowsAsync<RequestFaultException>(command);
        var exceptionName = exception.Fault.Exceptions.FirstOrDefault()!.ExceptionType;
        Assert.Contains(nameof(ValidationException), exceptionName);
    }

    [Fact]
    public async Task GivenICanCreateAUser_WhenICreateADuplicateUser_ThenIGetTheExistingUser()
    {
        // arrange
        var name = Guid.NewGuid().ToString();
        var user = await _client.GetResponse<CreateUserResponse>(new CreateUserCommand(name));

        // act
        var response = await _client.GetResponse<CreateUserResponse>(new CreateUserCommand(name));

        // assert
        Assert.Equal(user.Message.Id, response.Message.Id);
        Assert.False(response.Message.IsCreated);
    }
}

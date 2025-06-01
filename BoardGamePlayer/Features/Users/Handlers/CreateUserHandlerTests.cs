using FluentValidation;
using MediatR;
using Xunit;

namespace BoardGamePlayer.Features.Users.Handlers;

public class CreateUserHandlerTests(IMediator _mediator)
{
    [Fact]
    public async Task GivenICanCreateAUser_WhenICreateAUser_ThenIGetTheirId()
    {
        // arrange

        // act
        var response = await _mediator.Send(new CreateUserCommand(Guid.NewGuid().ToString()));

        // assert
        Assert.True(response.Id.ToString().Length > 0);
        Assert.True(response.IsCreated);
    }

    [Fact]
    public async Task GivenICanCreateAUser_WhenICreateAUserWithNoName_ThenIGetAValidationError()
    {
        // arrange

        // act
        var command = async () => await _mediator.Send(new CreateUserCommand(""));

        // assert
        await Assert.ThrowsAsync<ValidationException>(command);
    }

    [Fact]
    public async Task GivenICanCreateAUser_WhenICreateADuplicateUser_ThenIGetTheExistingUser()
    {
        // arrange
        var name = Guid.NewGuid().ToString();
        var user = await _mediator.Send(new CreateUserCommand(name));

        // act
        var response = await _mediator.Send(new CreateUserCommand(name));

        // assert
        Assert.Equal(user.Id, response.Id);
        Assert.False(response.IsCreated);
    }
}

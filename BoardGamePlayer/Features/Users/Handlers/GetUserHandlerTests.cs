using BoardGamePlayer.Infrastructure.Exceptions;
using MediatR;
using Xunit;

namespace BoardGamePlayer.Features.Users.Handlers;

public class GetUserHandlerTests(IMediator _mediator)
{
    [Fact]
    public async Task GivenIHaveAUser_WhenIGetTheUserById_ThenIGetTheUser()
    {
        // arrange
        var name = Guid.NewGuid().ToString();
        var savedUser = await _mediator.Send(new CreateUserCommand(name));

        // act
        var response = await _mediator.Send(new GetUserQuery(savedUser.Id, ""));

        // assert
        Assert.Equal(savedUser.Id, response.Id);
        Assert.Equal(name, response.Name);
    }

    [Fact]
    public async Task GivenIHaveAUser_WhenIGetTheUserByName_ThenIGetTheUser()
    {
        // arrange
        var name = Guid.NewGuid().ToString();
        var savedUser = await _mediator.Send(new CreateUserCommand(name));

        // act
        var response = await _mediator.Send(new GetUserQuery(default(Guid), name));

        // assert
        Assert.Equal(savedUser.Id, response.Id);
        Assert.Equal(name, response.Name);
    }

    [Fact]
    public async Task GivenIHaveNoUsers_WhenIGetAUserByName_ThenIGetANotFoundException()
    {
        // arrange
        var name = Guid.NewGuid().ToString();

        // act
        var command = async () => await _mediator.Send(new GetUserQuery(default(Guid), name));

        // assert
        await Assert.ThrowsAsync<NotFoundException>(command);
    }

    [Fact]
    public async Task GivenIHaveNoUsers_WhenIGetAUserById_ThenIGetANotFoundException()
    {
        // arrange
        var id = Guid.NewGuid();

        // act
        var command = async () => await _mediator.Send(new GetUserQuery(id, ""));

        // assert
        await Assert.ThrowsAsync<NotFoundException>(command);
    }

    [Fact]
    public async Task GivenIHaveAUser_WhenIGetAUserByWrongId_ThenIGetANotFoundException()
    {
        // arrange
        var invalidId = Guid.NewGuid();
        var name = Guid.NewGuid().ToString();
        await _mediator.Send(new CreateUserCommand(name));

        // act
        var command = async () => await _mediator.Send(new GetUserQuery(invalidId, ""));

        // assert
        await Assert.ThrowsAsync<NotFoundException>(command);
    }

    [Fact]
    public async Task GivenIHaveAUser_WhenIGetAUserByWrongName_ThenIGetANotFoundException()
    {
        // arrange
        var invalidName = Guid.NewGuid().ToString();
        var name = Guid.NewGuid().ToString();
        await _mediator.Send(new CreateUserCommand(name));

        // act
        var command = async () => await _mediator.Send(new GetUserQuery(default(Guid), invalidName));

        // assert
        await Assert.ThrowsAsync<NotFoundException>(command);
    }
}


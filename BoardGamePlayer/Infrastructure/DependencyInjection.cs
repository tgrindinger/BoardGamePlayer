using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using BoardGamePlayer.Features.Games;
using BoardGamePlayer.Features.Games.Handlers;
using BoardGamePlayer.Features.Users;
using BoardGamePlayer.Features.Users.Handlers;

namespace BoardGamePlayer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        // swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "VSA API", Version = "v1" });
        });
        services.RegisterAppDependencies();
        return services;
    }

    public static IServiceCollection RegisterAppDependencies(this IServiceCollection services)
    {
        // infrastructure
        services.AddTransient<IMediator, Mediator>();
        services.AddDbContext<GameAppDbContext>(options => options.UseInMemoryDatabase("TestDb-Game"));
        services.AddDbContext<UserAppDbContext>(options => options.UseInMemoryDatabase("TestDb-User"));

        // mapping
        services.AddScoped<IMappingMediator<LookupUserQuery, LookupUserResponse>,
            CustomMappingMediator<LookupUserQuery, LookupUserResponse, GetUserQuery, GetUserResponse>>();
        services.AddScopedMapping<LookupUserQuery, GetUserQuery>(
            source => new GetUserQuery(source.Id, string.Empty));
        services.AddScopedMapping<GetUserResponse, LookupUserResponse>(
            source => new LookupUserResponse(source.Id));

        // handlers
        services.AddScoped<IRequestHandler<CreateUserCommand, CreateUserResponse>, CreateUserHandler>();
        services.AddScoped<IRequestHandler<GetUserQuery, GetUserResponse>, GetUserHandler>();
        services.AddScoped<IRequestHandler<CreateGameCommand, CreateGameResponse>, CreateGameHandler>();
        services.AddScoped<IRequestHandler<GetGameQuery, GetGameResponse>, GetGameHandler>();

        // behaviors
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<CreateGameCommand>, CreateGameCommandValidator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using BoardGamePlayer.Features.Games.Handlers;
using BoardGamePlayer.Features.Users.Handlers;
using BoardGamePlayer.Data;

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
        services.AddDbContext<CommandDbContext>(options => options
            .UseInMemoryDatabase("TestDb"));
        services.AddDbContext<QueryDbContext>(options => options
            .UseInMemoryDatabase("TestDb")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        // handlers
        services.AddScoped<IRequestHandler<CreateUserCommand, CreateUserResponse>, CreateUserHandler>();
        services.AddScoped<IRequestHandler<GetUserQuery, GetUserResponse>, GetUserHandler>();
        services.AddScoped<IRequestHandler<CreateGameCommand, CreateGameResponse>, CreateGameHandler>();
        services.AddScoped<IRequestHandler<GetGameQuery, GetGameResponse>, GetGameHandler>();
        services.AddScoped<IRequestHandler<GetGamesQuery, GetGamesResponse>, GetGamesHandler>();
        services.AddScoped<IRequestHandler<StartGameCommand, StartGameResponse>, StartGameHandler>();

        // behaviors
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<CreateGameCommand>, CreateGameCommandValidator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}

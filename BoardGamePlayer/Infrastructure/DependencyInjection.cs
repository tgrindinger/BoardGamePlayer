using BoardGamePlayer.Data;
using BoardGamePlayer.Features.Games.Handlers;
using BoardGamePlayer.Features.Users.Handlers;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
        services.AddDbContext<CommandDbContext>(options => options
            .UseInMemoryDatabase("TestDb"));
        services.AddDbContext<QueryDbContext>(options => options
            .UseInMemoryDatabase("TestDb")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetExecutingAssembly());
            x.AddRequestClient<CreateGameCommand>();
            x.AddRequestClient<StartGameCommand>();
            x.AddRequestClient<GetGameQuery>();
            x.AddRequestClient<GetGamesQuery>();
            x.AddRequestClient<CreateUserCommand>();
            x.AddRequestClient<GetUserQuery>();
            x.UsingInMemory((context, cfg) =>
            {
                cfg.UseConsumeFilter(typeof(ValidationBehavior<>), context);
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService();

        // behaviors
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<CreateGameCommand>, CreateGameCommandValidator>();
        services.AddScoped<IValidator<StartGameCommand>, StartGameCommandValidator>();
        services.AddScoped<IValidator<GetGameQuery>, GetGameQueryValidator>();
        services.AddScoped<IValidator<GetGamesQuery>, GetGamesQueryValidator>();
        services.AddScoped<IValidator<GetUserQuery>, GetUserQueryValidator>();
        return services;
    }
}

using BoardGamePlayer.Data;
using BoardGamePlayer.Features.Games.Handlers;
using BoardGamePlayer.Features.Users.Handlers;
using FluentValidation;
using MassTransit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BoardGamePlayer.Infrastructure;

public static class DependencyInjection
{
    private static SqliteConnection? _keepAlive;

    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
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
        var connectionString = "DataSource=TestDb;mode=memory;cache=shared";
        _keepAlive = new SqliteConnection(connectionString);
        _keepAlive.Open();

        services.AddDbContext<CommandDbContext>(options => options
            .UseSqlite(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information));
        services.AddDbContext<QueryDbContext>(options => options
            .UseSqlite(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .LogTo(Console.WriteLine, LogLevel.Information));

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

        services.AddScoped<IValidator<CreateGameCommand>, CreateGameCommandValidator>();
        services.AddScoped<IValidator<StartGameCommand>, StartGameCommandValidator>();
        services.AddScoped<IValidator<GetGameQuery>, GetGameQueryValidator>();
        services.AddScoped<IValidator<GetGamesQuery>, GetGamesQueryValidator>();
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<GetUserQuery>, GetUserQueryValidator>();
        return services;
    }
}

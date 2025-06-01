using MediatR;

namespace BoardGamePlayer.Infrastructure;

internal static class TestStartup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient(_ =>
        {
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            var provider = new ServiceCollection()
                .RegisterAppDependencies()
                .BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            return provider.GetService<IMediator>()!;
        });
    }
}

using Xunit.DependencyInjection.AspNetCoreTesting;

namespace BoardGamePlayer.Infrastructure.Testing;

public static class TestStartup
{
    public static IHostBuilder CreateHostBuilder() => MinimalApiHostBuilderFactory.GetHostBuilder<Program>();
}

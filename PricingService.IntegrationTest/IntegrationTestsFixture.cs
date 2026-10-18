using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Xunit;

namespace PricingService.IntegrationTest;

public class IntegrationTestsFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer pgSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("lab_netmicro_pricing")
        .WithCleanUp(true)
        .Build();

    public IAlbaHost SystemUnderTest { get; private set; }

    public async Task InitializeAsync()
    {
        await pgSqlContainer.StartAsync();

        var hostBuilder = Program.CreateHostBuilder(Array.Empty<string>())
            .ConfigureServices((Action<IServiceCollection>)SetupServices)
            .ConfigureAppConfiguration((ctx, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = pgSqlContainer.GetConnectionString()
                });
            });

        SystemUnderTest = await AlbaHost.For(hostBuilder);
    }

    public async Task DisposeAsync()
    {
        await SystemUnderTest.DisposeAsync();
        await pgSqlContainer.DisposeAsync();
    }

    protected virtual void SetupServices(IServiceCollection services)
    {
    }

    protected Task SetupData()
    {
        return Task.CompletedTask;
    }
}
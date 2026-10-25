using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DashboardService.Implementation;
using DashboardService.Implementation.Messaging.RabbitMq;
using Steeltoe.Discovery.Client;

namespace DashboardService;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);

                    // Only apply docker overrides when actually running inside a container.
                    if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
                    {
                        config.AddJsonFile("appsettings.docker.json", true, true);
                    }

                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDiscoveryClient(hostContext.Configuration);

                    services.AddControllers().AddNewtonsoftJson();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();

                    services.AddDashboardService();
                    services.AddDashboardInfrastructure(hostContext.Configuration);
                })
                .Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                    if (env.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                    app.UseRabbitListeners(new List<Type> { typeof(PolicyService.Api.Events.PolicyCreated) });
                });
            });
    }
}
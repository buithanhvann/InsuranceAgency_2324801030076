using System;
using System.Text;
using System.Threading.Tasks;
using ChatService.Hubs;
using ChatService.Implementation;
using ChatService.Messaging.RabbitMq;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PolicyService.Api.Events;
using Steeltoe.Discovery.Client;

namespace ChatService;

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

                    if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
                    {
                        config.AddJsonFile("appsettings.docker.json", true, true);
                    }

                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Eureka service discovery (BTH6)
                    services.AddDiscoveryClient(hostContext.Configuration);

                    var appSettingsSection = hostContext.Configuration.GetSection("AppSettings");
                    services.Configure<AppSettings>(appSettingsSection);
                    var appSettings = appSettingsSection.Get<AppSettings>() ?? new AppSettings();

                    // MediatR: handlers in host (Chat) + implementation project
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
                    services.AddChatService();

                    services.AddCors(opt => opt.AddPolicy("CorsPolicy",
                        builder =>
                        {
                            builder
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins(appSettings.AllowedChatOrigins);
                        }));

                    services.AddControllers().AddNewtonsoftJson();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();

                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                    services.AddAuthentication(x =>
                        {
                            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(x =>
                        {
                            x.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(key),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                ValidateActor = false
                            };
                            x.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    var accessToken = context.Request.Query["access_token"];

                                    var path = context.HttpContext.Request.Path;
                                    if (!string.IsNullOrEmpty(accessToken) &&
                                        path.StartsWithSegments("/agentsChat"))
                                        context.Token = accessToken;
                                    return Task.CompletedTask;
                                }
                            };
                        });

                    services.AddAuthorization(_ => { });
                    services.AddSignalR();
                    services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

                    var rabbitOptions = hostContext.Configuration
                        .GetSection("RabbitMqOptions")
                        .Get<RabbitMqOptions>() ?? new RabbitMqOptions();
                    services.AddRabbitListeners(rabbitOptions);
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
                    app.UseCors("CorsPolicy");
                    app.UseAuthentication();
                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapHub<AgentChatHub>("/agentsChat");
                    });

                    app.UseRabbitListeners(new List<Type> { typeof(PolicyCreated) });
                });
            });
    }
}

using System;
using System.Text;
using AuthService.DataAccess;
using AuthService.Implementation;
using AuthDomain = AuthService.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Steeltoe.Discovery.Client;

namespace AuthService;

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
                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);

                    services.AddCors(opt => opt.AddPolicy("CorsPolicy",
                        builder =>
                        {
                            builder
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins(appSettings.AllowedAuthOrigins);
                        }));

                    services.AddControllers().AddNewtonsoftJson();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();

                    services.AddAuthentication(x =>
                        {
                            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(x =>
                        {
                            x.RequireHttpsMetadata = false;
                            x.SaveToken = true;
                            x.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(key),
                                ValidateIssuer = false,
                                ValidateAudience = false
                            };
                        });

                    services.AddSingleton<AuthDomain.AuthService>();
                    services.AddSingleton<AuthDomain.IInsuranceAgents, InsuranceAgentsInMemoryDb>();

                    // MediatR (BTH4 architecture)
                    services.AddAuthService();
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
                    });
                });
            });
    }
}

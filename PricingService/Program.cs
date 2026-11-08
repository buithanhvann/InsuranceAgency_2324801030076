using System.Reflection;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PricingService.Implementation;
using PricingService.Implementation.Data;
using PricingService.Api.Commands;
using FluentValidation;
using FluentValidation.Results;
using Steeltoe.Discovery.Client;

namespace PricingService;

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
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Add Steeltoe Discovery Client for Eureka registration
                    services.AddDiscoveryClient(hostContext.Configuration);

                    services.AddControllers().AddNewtonsoftJson();
                    services.AddValidatorsFromAssemblyContaining<CalculatePriceCommandValidator>();
                    services.AddScoped<IValidator<CalculatePriceCommand>, CalculatePriceCommandValidator>();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    
                    services.AddPricingService();
                    
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        services.AddDbContext<PricingDbContext>(options =>
                            options.UseNpgsql(connectionString));
                    }
                })
                .Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                    if (env.IsDevelopment() || env.IsEnvironment("docker"))
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    // UseDiscoveryClient() is obsolete in Steeltoe 3.x - registration happens automatically
                    // app.UseDiscoveryClient();

                    // Global exception handler for ValidationException - must be first middleware
                    app.Use(async (context, next) =>
                    {
                        try
                        {
                            await next();
                        }
                        catch (ValidationException ex)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            context.Response.ContentType = "application/json";
                            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                            await context.Response.WriteAsJsonAsync(new { Errors = errors });
                        }
                    });

                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
    }
}
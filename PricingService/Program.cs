using System.Reflection;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PricingService.Implementation;
using PricingService.Implementation.Data;
using PricingService.Api.Commands;
using FluentValidation.AspNetCore;
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

                    services.AddControllers(options =>
                    {
                        options.Filters.Add<ValidateModelFilter>();
                    }).AddNewtonsoftJson();
                    services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
                    services.AddValidatorsFromAssemblyContaining<CalculatePriceCommandValidator>();
                    services.AddScoped<IValidator<CalculatePriceCommand>, CalculatePriceCommandValidator>();
                    services.AddScoped<ValidateModelFilter>();
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
                    if (env.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    // UseDiscoveryClient() is obsolete in Steeltoe 3.x - registration happens automatically
                    // app.UseDiscoveryClient();

                    app.UseRouting();

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

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
    }
}

public class ValidateModelFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(e => e.Value.Errors.Select(err => new
                {
                    PropertyName = e.Key,
                    ErrorMessage = err.ErrorMessage
                }))
                .ToList();
            
            context.Result = new BadRequestObjectResult(new { Errors = errors });
            return;
        }
        
        await next();
    }
}
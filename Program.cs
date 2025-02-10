using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Hosting;

namespace UrlMin;

/// <summary>
/// Entry point for the URL shortener application
/// </summary>
public class Program
{
    /// <summary>
    /// Application entry point. Configures and starts the web host.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure services
        ConfigureServices(builder);

        // Build and configure the application
        var app = builder.Build();
        await ConfigureApplicationAsync(app);
    }

    /// <summary>
    /// Configures the application's services
    /// </summary>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Configure API controllers and JSON options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = 
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
            });

        // Add cross-origin resource sharing
        builder.Services.AddCors();

        // Add health monitoring
        builder.Services.AddHealthChecks();

        // Configure HTTP settings
        builder.Services.AddHttpsRedirection(options => options.HttpsPort = null);
        builder.WebHost.UseKestrel(options =>
        {
            options.ConfigureEndpointDefaults(config => config.Protocols = HttpProtocols.Http1);
        });
    }

    /// <summary>
    /// Configures the HTTP request pipeline and starts the application
    /// </summary>
    private static async Task ConfigureApplicationAsync(WebApplication app)
    {
        // Development-specific configuration
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Configure middleware pipeline
        ConfigureMiddleware(app);

        await app.RunAsync();
    }

    /// <summary>
    /// Configures the middleware pipeline
    /// </summary>
    private static void ConfigureMiddleware(WebApplication app)
    {
        // Static files and routing
        app.UseDefaultFiles()
           .UseStaticFiles()
           .UseRouting()
           .UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");

        // Security headers
        app.Use((context, next) =>
        {
            var headers = context.Response.Headers;
            headers.XFrameOptions = "DENY";
            headers.XContentTypeOptions = "nosniff";
            headers.XXSSProtection = "1; mode=block";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            headers.ContentSecurityPolicy = "default-src 'self'";
            headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
            return next();
        });

        // Request timing
        app.Use(async (context, next) =>
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                await next(context);
            }
            finally
            {
                timer.Stop();
                context.Response.Headers["X-Response-Time-Ms"] = 
                    timer.ElapsedMilliseconds.ToString();
            }
        });
    }
}

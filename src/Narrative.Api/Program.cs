using FastEndpoints;
using FastEndpoints.Swagger;
using Narrative.Content;

namespace Narrative.Api;

internal static class Program
{
    internal static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.ConfigureServices();

        WebApplication app = builder.Build();
        app.ConfigureMiddleware();
        app.Run();
    }

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        builder.Services
            .AddFastEndpoints()
            .SwaggerDocument(options =>
            {
                options.DocumentSettings = settings =>
                {
                    settings.Title = "Narrative API";
                    settings.Version = "v1";
                    settings.Description = "REST API for Narrative web application.";
                };
            });

        builder.Services.AddContentModule(builder.Configuration);
    }

    private static void ConfigureMiddleware(this WebApplication app)
    {
        app.MapOpenApi();

        app
            .UseFastEndpoints(config => config.Endpoints.RoutePrefix = "api")
            .UseSwaggerGen();
    }
}
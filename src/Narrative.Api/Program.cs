using FastEndpoints;
using FastEndpoints.Swagger;
using Narrative.Content;

namespace Narrative.Api;

internal static class Program
{
    internal static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.ConfigureServices();

        WebApplication app = builder.Build();
        app.ConfigureMiddleware();
        app.Run();
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddOpenApi();

        services
            .AddFastEndpoints()
            .SwaggerDocument();

        services.AddContentModule();

        return services;
    }

    private static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app
            .UseFastEndpoints(config => config.Endpoints.RoutePrefix = "api")
            .UseSwaggerGen();
    }
}
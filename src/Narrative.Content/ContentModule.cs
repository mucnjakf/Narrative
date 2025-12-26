using Microsoft.Extensions.DependencyInjection;
using Narrative.Content.Data;

namespace Narrative.Content;

public static class ContentModule
{
    public static IServiceCollection AddContentModule(this IServiceCollection services)
    {
        services.AddScoped<IArticleRepository, TestArticleRepository>();

        return services;
    }
}
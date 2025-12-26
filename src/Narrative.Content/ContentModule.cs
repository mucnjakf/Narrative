using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narrative.Content.Data;

namespace Narrative.Content;

public static class ContentModule
{
    public static IServiceCollection AddContentModule(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = Guard.Against.NullOrEmpty(
            configuration.GetConnectionString("ContentModule"));

        services.AddDbContext<ContentDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IArticleRepository, EfCoreArticleRepository>();

        return services;
    }
}
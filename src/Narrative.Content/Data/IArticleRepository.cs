using Narrative.Content.Domain;

namespace Narrative.Content.Data;

internal interface IArticleRepository
{
    Task<Article?> FindOrDefaultAsync(Guid id);

    Task CreateAsync(Article article);

    Task SaveChangesAsync();
}
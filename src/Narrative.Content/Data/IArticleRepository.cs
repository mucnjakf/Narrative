using Narrative.Content.Domain;

namespace Narrative.Content.Data;

internal interface IArticleRepository
{
    Task<List<Article>> GetAllAsync();

    Task<Article?> FindOrDefaultAsync(Guid id);

    Task CreateAsync(Article article);

    Task Delete(Article article);

    Task SaveChangesAsync();
}
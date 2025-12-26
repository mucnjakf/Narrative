using Narrative.Content.Domain;

namespace Narrative.Content.Data;

internal sealed class EfCoreArticleRepository(ContentDbContext dbContext) : IArticleRepository
{
    public async Task<Article?> FindOrDefaultAsync(Guid id) => await dbContext.Articles.FindAsync(id);

    public async Task CreateAsync(Article article) => await dbContext.Articles.AddAsync(article);

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
}
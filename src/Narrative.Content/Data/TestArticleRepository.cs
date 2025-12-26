using Narrative.Content.Domain;

namespace Narrative.Content.Data;

internal sealed class TestArticleRepository : IArticleRepository
{
    private static readonly List<Article> Articles = [];

    public Task<Article?> FindOrDefaultAsync(Guid id)
        => Task.FromResult(Articles.FirstOrDefault(x => x.Id == id));

    public Task CreateAsync(Article article)
    {
        Articles.Add(article);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => Task.CompletedTask;
}
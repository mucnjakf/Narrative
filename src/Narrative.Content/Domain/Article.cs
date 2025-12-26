using Ardalis.GuardClauses;
using Narrative.Content.Enums;

namespace Narrative.Content.Domain;

internal sealed class Article
{
    internal Guid Id { get; private set; }

    internal DateTimeOffset CreatedAtUtc { get; private set; }

    internal DateTimeOffset UpdatedAtUtc { get; private set; }

    internal string Title { get; private set; }

    internal string Description { get; private set; }

    internal string Content { get; private set; }

    internal DateTimeOffset? PublishedAtUtc { get; private set; }

    internal ArticleStatus Status { get; private set; }

    internal Article(string title, string description, string content)
    {
        Id = Guid.NewGuid();
        CreatedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
        PublishedAtUtc = null;
        Status = ArticleStatus.Pending;

        Title = Guard.Against.NullOrEmpty(title);
        Description = Guard.Against.NullOrEmpty(description);
        Content = Guard.Against.NullOrEmpty(content);
    }
}
using Narrative.Content.Enums;

namespace Narrative.Content.Dtos;

internal sealed record ArticleDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc,
    string Title,
    string Description,
    string Content,
    DateTimeOffset? PublishedAtUtc,
    ArticleStatus Status);
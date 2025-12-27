using Narrative.Content.Enums;
using Narrative.Shared;

namespace Narrative.Content.Domain;

internal sealed class ArticleErrors
{
    public static Error DecisionCannotBeUsedForReview(ArticleStatus decision)
        => Error.Validation(
            "Article.DecisionCannotBeUsedForReview",
            $"Decision '{decision}' cannot be used for review.");

    public static Error ArticleWithStatusCannotBeReviewed(Guid id, ArticleStatus status)
        => Error.Validation(
            "Article.ArticleWithStatusCannotBeReviewed",
            $"Article with ID '{id}' and status '{status}' cannot be reviewed.");

    public static Error ArticleNotFound(Guid id)
        => Error.NotFound(
            "Article.ArticleNotFound",
            $"Article with ID '{id}' not found.");
}
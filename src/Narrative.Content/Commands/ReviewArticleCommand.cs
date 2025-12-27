using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Content.Enums;
using Narrative.Shared;

namespace Narrative.Content.Commands;

internal sealed record ReviewArticleCommand(Guid Id, ArticleStatus Decision) : ICommand<Result>;

internal sealed class ReviewArticleCommandHandler(
    ILogger<ReviewArticleCommandHandler> logger,
    IArticleRepository articleRepository)
    : ICommandHandler<ReviewArticleCommand, Result>
{
    public async Task<Result> ExecuteAsync(ReviewArticleCommand command, CancellationToken ct)
    {
        Article? article = await articleRepository.FindOrDefaultAsync(command.Id);

        if (article is null)
        {
            Error error = ArticleErrors.ArticleNotFound(command.Id);
            logger.LogError("Code: {Code} - Description: {Description}", error.Code, error.Description);

            return Result.Failure(error);
        }

        Result result = article.Review(command.Decision);

        if (result.IsFailure)
        {
            logger.LogError("Code: {Code} - Description: {Description}", result.Error.Code, result.Error.Description);
            return result;
        }

        await articleRepository.SaveChangesAsync();

        return result;
    }
}
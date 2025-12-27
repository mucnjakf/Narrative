using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Content.Dtos;
using Narrative.Shared;

namespace Narrative.Content.Commands;

internal sealed record GetArticleCommand(Guid Id) : ICommand<Result<ArticleDto>>;

internal sealed class GetArticleCommandHandler(
    ILogger<GetArticleCommandHandler> logger,
    IArticleRepository articleRepository)
    : ICommandHandler<GetArticleCommand, Result<ArticleDto>>
{
    public async Task<Result<ArticleDto>> ExecuteAsync(GetArticleCommand command, CancellationToken ct)
    {
        Article? article = await articleRepository.FindOrDefaultAsync(command.Id);

        if (article is null)
        {
            Error error = ArticleErrors.ArticleNotFound(command.Id);
            logger.LogError("Code: {Code} - Description: {Description}", error.Code, error.Description);

            return Result.Failure<ArticleDto>(error);
        }

        return Result.Success(
            new ArticleDto(
                article.Id,
                article.CreatedAtUtc,
                article.UpdatedAtUtc,
                article.Title,
                article.Description,
                article.Content,
                article.PublishedAtUtc,
                article.Status));
    }
}
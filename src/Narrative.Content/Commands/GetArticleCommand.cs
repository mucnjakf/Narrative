using Ardalis.Result;
using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Content.Dtos;

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
            logger.LogError("Article with ID {Id} not found.", command.Id);
            return Result.NotFound($"Article with ID {command.Id} not found.");
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
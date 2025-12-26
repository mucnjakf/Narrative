using Ardalis.Result;
using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Content.Dtos;

namespace Narrative.Content.Commands;

internal sealed record GetAllArticlesCommand : ICommand<Result<List<ArticleDto>>>;

internal sealed class GetAllArticlesCommandHandler(
    ILogger<GetAllArticlesCommandHandler> logger,
    IArticleRepository articleRepository)
    : ICommandHandler<GetAllArticlesCommand, Result<List<ArticleDto>>>
{
    public async Task<Result<List<ArticleDto>>> ExecuteAsync(GetAllArticlesCommand command, CancellationToken ct)
    {
        List<Article> articles = await articleRepository.GetAllAsync();

        List<ArticleDto> articleDtos = articles
            .Select(article => new ArticleDto(
                article.Id,
                article.CreatedAtUtc,
                article.UpdatedAtUtc,
                article.Title,
                article.Description,
                article.Content,
                article.PublishedAtUtc,
                article.Status))
            .ToList();

        logger.LogInformation("{Count} articles found.", articleDtos.Count);

        return Result.Success(articleDtos);
    }
}
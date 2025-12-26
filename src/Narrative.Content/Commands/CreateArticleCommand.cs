using Ardalis.Result;
using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Content.Dtos;

namespace Narrative.Content.Commands;

internal sealed record CreateArticleCommand(string Title, string Description, string Content)
    : ICommand<Result<ArticleDto>>;

internal sealed class CreateArticleCommandHandler(
    ILogger<CreateArticleCommandHandler> logger,
    IArticleRepository articleRepository)
    : ICommandHandler<CreateArticleCommand, Result<ArticleDto>>
{
    public async Task<Result<ArticleDto>> ExecuteAsync(CreateArticleCommand command, CancellationToken ct)
    {
        var article = new Article(command.Title, command.Description, command.Content);

        await articleRepository.CreateAsync(article);
        await articleRepository.SaveChangesAsync();

        logger.LogInformation("Article with ID {Id} successfully created.", article.Id);

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
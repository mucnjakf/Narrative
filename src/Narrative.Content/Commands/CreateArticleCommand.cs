using FastEndpoints;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Content.Dtos;
using Narrative.Shared;

namespace Narrative.Content.Commands;

internal sealed record CreateArticleCommand(string Title, string Description, string Content)
    : ICommand<Result<ArticleDto>>;

internal sealed class CreateArticleCommandHandler(IArticleRepository articleRepository)
    : ICommandHandler<CreateArticleCommand, Result<ArticleDto>>
{
    public async Task<Result<ArticleDto>> ExecuteAsync(CreateArticleCommand command, CancellationToken ct)
    {
        var article = new Article(command.Title, command.Description, command.Content);

        await articleRepository.CreateAsync(article);
        await articleRepository.SaveChangesAsync();

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
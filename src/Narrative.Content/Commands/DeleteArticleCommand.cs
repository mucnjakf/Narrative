using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Shared;

namespace Narrative.Content.Commands;

internal sealed record DeleteArticleCommand(Guid Id) : ICommand<Result>;

internal sealed class DeleteArticleCommandHandler(
    ILogger<DeleteArticleCommandHandler> logger,
    IArticleRepository articleRepository)
    : ICommandHandler<DeleteArticleCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteArticleCommand command, CancellationToken ct)
    {
        Article? article = await articleRepository.FindOrDefaultAsync(command.Id);

        if (article is null)
        {
            Error error = ArticleErrors.ArticleNotFound(command.Id);
            logger.LogError("Code: {Code} - Description: {Description}", error.Code, error.Description);

            return Result.Failure(error);
        }

        await articleRepository.Delete(article);
        await articleRepository.SaveChangesAsync();

        return Result.Success();
    }
}
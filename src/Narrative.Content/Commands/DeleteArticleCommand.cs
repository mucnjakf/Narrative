using Ardalis.Result;
using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;

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
            logger.LogError("Article with ID {Id} not found.", command.Id);
            return Result.NotFound($"Article with ID {command.Id} not found.");
        }

        await articleRepository.Delete(article);
        await articleRepository.SaveChangesAsync();

        logger.LogInformation("Article with ID {Id} successfully deleted.", article.Id);

        return Result.Success();
    }
}
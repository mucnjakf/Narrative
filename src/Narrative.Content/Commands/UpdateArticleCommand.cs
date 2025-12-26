using Ardalis.Result;
using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;

namespace Narrative.Content.Commands;

internal sealed record UpdateArticleCommand(Guid Id, string Title, string Description, string Content)
    : ICommand<Result>;

internal sealed class UpdateArticleCommandHandler(
    ILogger<UpdateArticleCommandHandler> logger,
    IArticleRepository articleRepository)
    : ICommandHandler<UpdateArticleCommand, Result>
{
    public async Task<Result> ExecuteAsync(UpdateArticleCommand command, CancellationToken ct)
    {
        Article? article = await articleRepository.FindOrDefaultAsync(command.Id);

        if (article is null)
        {
            logger.LogError("Article with ID {Id} not found.", command.Id);
            return Result.NotFound($"Article with ID {command.Id} not found.");
        }

        article.Update(command.Title, command.Description, command.Content);
        await articleRepository.SaveChangesAsync();

        logger.LogInformation("Article with ID {Id} successfully updated.", article.Id);

        return Result.Success();
    }
}
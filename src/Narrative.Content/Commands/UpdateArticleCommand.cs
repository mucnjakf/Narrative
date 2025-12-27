using FastEndpoints;
using Microsoft.Extensions.Logging;
using Narrative.Content.Data;
using Narrative.Content.Domain;
using Narrative.Shared;

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
            Error error = ArticleErrors.ArticleNotFound(command.Id);
            logger.LogError("Code: {Code} - Description: {Description}", error.Code, error.Description);

            return Result.Failure(error);
        }

        article.Update(command.Title, command.Description, command.Content);
        await articleRepository.SaveChangesAsync();

        return Result.Success();
    }
}
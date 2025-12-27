using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

internal sealed record UpdateArticleRequest(Guid Id, string Title, string Description, string Content);

internal sealed class UpdateArticleEndpoint : Endpoint<UpdateArticleRequest, Results<NoContent, ProblemHttpResult>>
{
    public override void Configure()
    {
        Put("articles/{id}");
        AllowAnonymous();
        Description(builder => builder.WithName("UpdateArticle"));
    }

    public override async Task<Results<NoContent, ProblemHttpResult>> ExecuteAsync(
        UpdateArticleRequest request,
        CancellationToken ct)
    {
        Result result = await new UpdateArticleCommand(request.Id, request.Title, request.Description, request.Content)
            .ExecuteAsync(ct);

        return result.IsSuccess ? TypedResults.NoContent() : result.ToProblemDetails();
    }
}
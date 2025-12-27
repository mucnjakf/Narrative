using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

internal sealed record DeleteArticleRequest(Guid Id);

internal sealed class DeleteArticleEndpoint : Endpoint<DeleteArticleRequest, Results<NoContent, ProblemHttpResult>>
{
    public override void Configure()
    {
        Delete("articles/{id}");
        AllowAnonymous();
        Description(builder => builder.WithName("DeleteArticle"));
    }

    public override async Task<Results<NoContent, ProblemHttpResult>> ExecuteAsync(
        DeleteArticleRequest request,
        CancellationToken ct)
    {
        Result result = await new DeleteArticleCommand(request.Id).ExecuteAsync(ct);

        return result.IsSuccess ? TypedResults.NoContent() : result.ToProblemDetails();
    }
}
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Content.Enums;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

internal sealed record ReviewArticleRequest(Guid Id, ArticleStatus Decision);

internal sealed class ReviewArticleEndpoint : Endpoint<ReviewArticleRequest, Results<NoContent, ProblemHttpResult>>
{
    public override void Configure()
    {
        Patch("articles/{id}/review");
        AllowAnonymous();
        Description(builder => builder.WithName("ReviewArticle"));
    }

    public override async Task<Results<NoContent, ProblemHttpResult>> ExecuteAsync(
        ReviewArticleRequest request,
        CancellationToken ct)
    {
        Result result = await new ReviewArticleCommand(request.Id, request.Decision).ExecuteAsync(ct);

        return result.IsSuccess ? TypedResults.NoContent() : result.ToProblemDetails();
    }
}
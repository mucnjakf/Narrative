using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Content.Dtos;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

internal sealed record GetArticleRequest(Guid Id);

internal sealed record GetArticleResponse(ArticleDto Article);

internal sealed class GetArticleEndpoint
    : Endpoint<GetArticleRequest, Results<Ok<GetArticleResponse>, ProblemHttpResult>>
{
    public override void Configure()
    {
        Get("articles/{id}");
        AllowAnonymous();
        Description(builder => builder.WithName("GetArticle"));
    }

    public override async Task<Results<Ok<GetArticleResponse>, ProblemHttpResult>> ExecuteAsync(
        GetArticleRequest request,
        CancellationToken ct)
    {
        Result<ArticleDto> result = await new GetArticleCommand(request.Id).ExecuteAsync(ct);

        return result.IsSuccess
            ? TypedResults.Ok(new GetArticleResponse(result.Value))
            : result.ToProblemDetails();
    }
}
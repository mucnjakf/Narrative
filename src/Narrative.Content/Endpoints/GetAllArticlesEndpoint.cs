using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Content.Dtos;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

internal sealed record GetAllArticlesResponse(List<ArticleDto> Articles);

internal sealed class GetAllArticlesEndpoint
    : EndpointWithoutRequest<Results<Ok<GetAllArticlesResponse>, ProblemHttpResult>>
{
    public override void Configure()
    {
        Get("articles");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<GetAllArticlesResponse>, ProblemHttpResult>> ExecuteAsync(
        CancellationToken ct)
    {
        Result<List<ArticleDto>> result = await new GetAllArticlesCommand().ExecuteAsync(ct);

        return result.IsSuccess
            ? TypedResults.Ok(new GetAllArticlesResponse(result.Value))
            : result.ToProblemDetails();
    }
}
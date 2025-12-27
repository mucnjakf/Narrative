using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Content.Dtos;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

internal sealed record CreateArticleRequest(string Title, string Description, string Content);

internal sealed record CreateArticleResponse(ArticleDto Article);

internal sealed class CreateArticleEndpoint
    : Endpoint<CreateArticleRequest, Results<CreatedAtRoute<CreateArticleResponse>, ProblemHttpResult>>
{
    public override void Configure()
    {
        Post("articles");
        AllowAnonymous();
        Description(builder => builder.WithName("CreateArticle"));
    }

    public override async Task<Results<CreatedAtRoute<CreateArticleResponse>, ProblemHttpResult>> ExecuteAsync(
        CreateArticleRequest request,
        CancellationToken ct)
    {
        Result<ArticleDto> result = await new CreateArticleCommand(request.Title, request.Description, request.Content)
            .ExecuteAsync(ct);

        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(
                new CreateArticleResponse(result.Value),
                "GetArticle",
                new { result.Value.Id })
            : result.ToProblemDetails();
    }
}
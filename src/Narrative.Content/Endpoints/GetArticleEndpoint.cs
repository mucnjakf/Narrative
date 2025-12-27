using System.Net.Mime;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Content.Dtos;
using Narrative.Content.Enums;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

/// <summary>
/// Get article request.
/// </summary>
/// <param name="Id">Article ID - <see cref="Guid"/></param>
internal sealed record GetArticleRequest(Guid Id);

/// <summary>
/// Get article response.
/// </summary>
/// <param name="Article">Article DTO - <see cref="ArticleDto"/></param>
internal sealed record GetArticleResponse(ArticleDto Article);

/// <summary>
/// Get article endpoint.
/// </summary>
internal sealed class GetArticleEndpoint
    : Endpoint<GetArticleRequest, Results<Ok<GetArticleResponse>, ProblemHttpResult>>
{
    /// <summary>
    /// Execute get article endpoint.
    /// </summary>
    /// <param name="request">Get article request - <see cref="GetArticleRequest"/></param>
    /// <param name="ct">Cancellation token - <see cref="CancellationToken"/></param>
    /// <returns>Ok with get article response or problem details - <see cref="Results{Ok,ProblemHttpResult}"/></returns>
    public override async Task<Results<Ok<GetArticleResponse>, ProblemHttpResult>> ExecuteAsync(
        GetArticleRequest request,
        CancellationToken ct)
    {
        Result<ArticleDto> result = await new GetArticleCommand(request.Id).ExecuteAsync(ct);

        return result.IsSuccess
            ? TypedResults.Ok(new GetArticleResponse(result.Value))
            : result.ToProblemDetails();
    }

    /// <summary>
    /// Configures get article endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("articles/{id}");

        AllowAnonymous();

        Summary(builder =>
        {
            builder.Summary = "Endpoint for getting article by ID.";

            builder.ExampleRequest = new GetArticleRequest(Guid.NewGuid());

            builder.RequestParam(property => property.Id, "Article ID.");

            builder.Response(
                StatusCodes.Status200OK,
                "Ok response - article.",
                example: new GetArticleResponse(
                    new ArticleDto(
                        Guid.NewGuid(),
                        DateTimeOffset.UtcNow,
                        DateTimeOffset.UtcNow,
                        "Example title",
                        "Example description",
                        "Example content",
                        null,
                        ArticleStatus.Pending)
                ));

            builder.Response(
                StatusCodes.Status404NotFound,
                "Not found response - article not found.",
                MediaTypeNames.Application.ProblemJson,
                TypedResults.Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Not Found")
                    .ProblemDetails);

            builder.Response(
                StatusCodes.Status500InternalServerError,
                "Internal server error response - unknown error occured.",
                MediaTypeNames.Application.ProblemJson,
                TypedResults.Problem(
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Internal Server Error")
                    .ProblemDetails);
        });
    }
}
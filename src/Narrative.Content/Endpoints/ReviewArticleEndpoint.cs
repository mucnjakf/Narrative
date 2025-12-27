using System.Net.Mime;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Content.Enums;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

/// <summary>
/// Review article request
/// </summary>
/// <param name="Id">Article ID - <see cref="Guid"/></param>
/// <param name="Decision">Article status - <see cref="ArticleStatus"/></param>
internal sealed record ReviewArticleRequest(Guid Id, ArticleStatus Decision);

/// <summary>
/// Review article endpoint.
/// </summary>
internal sealed class ReviewArticleEndpoint : Endpoint<ReviewArticleRequest, Results<NoContent, ProblemHttpResult>>
{
    /// <summary>
    /// Execute review article endpoint.
    /// </summary>
    /// <param name="request">Review article request - <see cref="ReviewArticleRequest"/></param>
    /// <param name="ct">Cancellation token - <see cref="CancellationToken"/></param>
    /// <returns>No content or problem details - <see cref="Results{NoContent,ProblemHttpResult}"/></returns>
    public override async Task<Results<NoContent, ProblemHttpResult>> ExecuteAsync(
        ReviewArticleRequest request,
        CancellationToken ct)
    {
        Result result = await new ReviewArticleCommand(request.Id, request.Decision).ExecuteAsync(ct);

        return result.IsSuccess ? TypedResults.NoContent() : result.ToProblemDetails();
    }

    /// <summary>
    /// Configures review article endpoint.
    /// </summary>
    public override void Configure()
    {
        Patch("articles/{id}/review");

        AllowAnonymous();

        Summary(builder =>
        {
            builder.Summary = "Endpoint for reviewing article.";

            builder.ExampleRequest = new ReviewArticleRequest(Guid.NewGuid(), ArticleStatus.Approved);

            builder.RequestParam(property => property.Id, "Article ID.");
            builder.RequestParam(property => property.Decision, "Article status.");

            builder.Response(
                StatusCodes.Status204NoContent,
                "No content response.");

            builder.Response(
                StatusCodes.Status400BadRequest,
                "Bad request response - validation errors, decision cannot be used for review or article with status cannot be reviewed.",
                MediaTypeNames.Application.ProblemJson,
                TypedResults.Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Bad Request")
                    .ProblemDetails);

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
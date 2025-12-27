using System.Net.Mime;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

/// <summary>
/// Delete article request.
/// </summary>
/// <param name="Id">Article ID - <see cref="Guid"/></param>
internal sealed record DeleteArticleRequest(Guid Id);

/// <summary>
/// Delete article endpoint.
/// </summary>
internal sealed class DeleteArticleEndpoint : Endpoint<DeleteArticleRequest, Results<NoContent, ProblemHttpResult>>
{
    /// <summary>
    /// Execute delete article endpoint.
    /// </summary>
    /// <param name="request">Delete article request - <see cref="DeleteArticleRequest"/></param>
    /// <param name="ct">Cancellation token - <see cref="CancellationToken"/></param>
    /// <returns>No content or problem details - <see cref="Results{NoContent,ProblemHttpResult}"/></returns>
    public override async Task<Results<NoContent, ProblemHttpResult>> ExecuteAsync(
        DeleteArticleRequest request,
        CancellationToken ct)
    {
        Result result = await new DeleteArticleCommand(request.Id).ExecuteAsync(ct);

        return result.IsSuccess ? TypedResults.NoContent() : result.ToProblemDetails();
    }

    /// <summary>
    /// Configures delete article endpoint.
    /// </summary>
    public override void Configure()
    {
        Delete("articles/{id}");

        AllowAnonymous();

        Summary(builder =>
        {
            builder.Summary = "Endpoint for deleting article.";

            builder.ExampleRequest = new DeleteArticleRequest(Guid.NewGuid());

            builder.RequestParam(property => property.Id, "Article ID.");

            builder.Response(
                StatusCodes.Status204NoContent,
                "No content response.");

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
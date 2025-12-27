using System.Net.Mime;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Narrative.Content.Commands;
using Narrative.Shared;

namespace Narrative.Content.Endpoints;

/// <summary>
/// Update article request.
/// </summary>
/// <param name="Id">Article ID - <see cref="Guid"/></param>
/// <param name="Title">Article title - <see cref="string"/></param>
/// <param name="Description">Article description - <see cref="string"/></param>
/// <param name="Content">Article content - <see cref="string"/></param>
internal sealed record UpdateArticleRequest(Guid Id, string Title, string Description, string Content);

/// <summary>
/// Update article endpoint.
/// </summary>
internal sealed class UpdateArticleEndpoint : Endpoint<UpdateArticleRequest, Results<NoContent, ProblemHttpResult>>
{
    /// <summary>
    /// Execute update article endpoint.
    /// </summary>
    /// <param name="request">Update article request - <see cref="UpdateArticleRequest"/></param>
    /// <param name="ct">Cancellation token - <see cref="CancellationToken"/></param>
    /// <returns>No content or problem details - <see cref="Results{NoContent,ProblemHttpResult}"/></returns>
    public override async Task<Results<NoContent, ProblemHttpResult>> ExecuteAsync(
        UpdateArticleRequest request,
        CancellationToken ct)
    {
        Result result = await new UpdateArticleCommand(request.Id, request.Title, request.Description, request.Content)
            .ExecuteAsync(ct);

        return result.IsSuccess ? TypedResults.NoContent() : result.ToProblemDetails();
    }

    /// <summary>
    /// Configures update article endpoint.
    /// </summary>
    public override void Configure()
    {
        Put("articles/{id}");

        AllowAnonymous();

        Summary(builder =>
        {
            builder.Summary = "Endpoint for updating an article.";

            builder.ExampleRequest = new UpdateArticleRequest(
                Guid.NewGuid(),
                "Example title",
                "Example description",
                "Example content");

            builder.RequestParam(property => property.Id, "Article ID.");
            builder.RequestParam(property => property.Title, "Article title.");
            builder.RequestParam(property => property.Description, "Article description.");
            builder.RequestParam(property => property.Content, "Article content.");

            builder.Response(
                StatusCodes.Status204NoContent,
                "No content response.");

            builder.Response(
                StatusCodes.Status400BadRequest,
                "Bad request response - validation errors.",
                MediaTypeNames.Application.ProblemJson,
                TypedResults.Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Bad Request")
                    .ProblemDetails);

            builder.Response(
                StatusCodes.Status404NotFound,
                "Not found - article not found.",
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
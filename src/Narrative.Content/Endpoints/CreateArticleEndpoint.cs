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
/// Create article request.
/// </summary>
/// <param name="Title">Article title - <see cref="string"/></param>
/// <param name="Description">Article description - <see cref="string"/></param>
/// <param name="Content">Article content - <see cref="string"/></param>
internal sealed record CreateArticleRequest(string Title, string Description, string Content);

/// <summary>
/// Create article response.
/// </summary>
/// <param name="Article">Article DTO - <see cref="ArticleDto"/></param>
internal sealed record CreateArticleResponse(ArticleDto Article);

/// <summary>
/// Create article endpoint.
/// </summary>
internal sealed class CreateArticleEndpoint
    : Endpoint<CreateArticleRequest, Results<CreatedAtRoute<CreateArticleResponse>, ProblemHttpResult>>
{
    /// <summary>
    /// Execute create article endpoint.
    /// </summary>
    /// <param name="request">Create article request - <see cref="CreateArticleRequest"/></param>
    /// <param name="ct">Cancellation token - <see cref="CancellationToken"/></param>
    /// <returns>Created at route with create article response or problem details - <see cref="Results{CreatedAtRoute,ProblemHttpResult}"/></returns>
    public override async Task<Results<CreatedAtRoute<CreateArticleResponse>, ProblemHttpResult>> ExecuteAsync(
        CreateArticleRequest request,
        CancellationToken ct)
    {
        Result<ArticleDto> result = await new CreateArticleCommand(request.Title, request.Description, request.Content)
            .ExecuteAsync(ct);

        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(
                new CreateArticleResponse(result.Value),
                IEndpoint.GetName<GetArticleEndpoint>(),
                new { result.Value.Id })
            : result.ToProblemDetails();
    }

    /// <summary>
    /// Configures create article endpoint.
    /// </summary>
    public override void Configure()
    {
        Post("articles");

        AllowAnonymous();

        Summary(builder =>
        {
            builder.Summary = "Endpoint for creating an article.";

            builder.ExampleRequest = new CreateArticleRequest(
                "Example title",
                "Example description",
                "Example content");

            builder.RequestParam(property => property.Title, "Article title.");
            builder.RequestParam(property => property.Description, "Article description.");
            builder.RequestParam(property => property.Content, "Article content.");

            builder.Response(StatusCodes.Status201Created,
                "Created at response - article.",
                example: new CreateArticleResponse(
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
                StatusCodes.Status400BadRequest,
                "Bad request response - validation errors.",
                MediaTypeNames.Application.ProblemJson,
                TypedResults.Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Bad Request")
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
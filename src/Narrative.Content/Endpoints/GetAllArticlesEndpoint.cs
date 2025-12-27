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
/// Get all articles response.
/// </summary>
/// <param name="Articles">List of article DTOs - <see cref="List{ArticleDto}"/></param>
internal sealed record GetAllArticlesResponse(List<ArticleDto> Articles);

/// <summary>
/// Get all articles endpoint.
/// </summary>
internal sealed class GetAllArticlesEndpoint
    : EndpointWithoutRequest<Results<Ok<GetAllArticlesResponse>, ProblemHttpResult>>
{
    /// <summary>
    /// Execute get all articles endpoint.
    /// </summary>
    /// <param name="ct">Cancellation token - <see cref="CancellationToken"/></param>
    /// <returns>Ok with get all articles response or problem details - <see cref="Results{Ok,ProblemHttpResult}"/></returns>
    public override async Task<Results<Ok<GetAllArticlesResponse>, ProblemHttpResult>> ExecuteAsync(
        CancellationToken ct)
    {
        Result<List<ArticleDto>> result = await new GetAllArticlesCommand().ExecuteAsync(ct);

        return result.IsSuccess
            ? TypedResults.Ok(new GetAllArticlesResponse(result.Value))
            : result.ToProblemDetails();
    }

    /// <summary>
    /// Configures get all article endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("articles");

        AllowAnonymous();

        Summary(builder =>
        {
            builder.Summary = "Endpoint for getting all articles.";

            builder.Response(StatusCodes.Status200OK,
                "Ok response - list of articles.",
                example: new GetAllArticlesResponse([
                    new ArticleDto(
                        Guid.NewGuid(),
                        DateTimeOffset.UtcNow,
                        DateTimeOffset.UtcNow,
                        "Example title",
                        "Example description",
                        "Example content",
                        null,
                        ArticleStatus.Pending)
                ]));

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
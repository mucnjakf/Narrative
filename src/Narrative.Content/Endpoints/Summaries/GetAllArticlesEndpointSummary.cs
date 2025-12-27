using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Narrative.Content.Dtos;
using Narrative.Content.Enums;

namespace Narrative.Content.Endpoints.Summaries;

internal sealed class GetAllArticlesEndpointSummary : Summary<GetAllArticlesEndpoint>
{
    public GetAllArticlesEndpointSummary()
    {
        Summary = "Endpoint for getting all articles.";

        Response(
            StatusCodes.Status200OK,
            "OK response - list of articles.",
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

        Response(
            StatusCodes.Status500InternalServerError,
            "Internal server error response - unknown error occured.",
            example: TypedResults.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error")
                .ProblemDetails);
    }
}
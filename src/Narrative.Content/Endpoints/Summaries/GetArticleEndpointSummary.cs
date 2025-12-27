using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Narrative.Content.Dtos;
using Narrative.Content.Enums;

namespace Narrative.Content.Endpoints.Summaries;

internal sealed class GetArticleEndpointSummary : Summary<GetArticleEndpoint>
{
    public GetArticleEndpointSummary()
    {
        Summary = "Endpoint for getting article by ID.";

        ExampleRequest = new GetArticleRequest(Guid.NewGuid());

        Response(
            StatusCodes.Status200OK,
            "OK response - article.",
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

        Response(
            StatusCodes.Status404NotFound,
            "Not found response - article not found.",
            example: TypedResults.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found")
                .ProblemDetails);

        Response(
            StatusCodes.Status500InternalServerError,
            "Internal server error response - unknown error occured.",
            example: TypedResults.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error")
                .ProblemDetails);
    }
}
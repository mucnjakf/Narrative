using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Narrative.Content.Dtos;
using Narrative.Content.Enums;

namespace Narrative.Content.Endpoints.Summaries;

internal sealed class CreateArticleEndpointSummary : Summary<CreateArticleEndpoint, CreateArticleRequest>
{
    public CreateArticleEndpointSummary()
    {
        Summary = "Endpoint for creating an article.";

        ExampleRequest = new CreateArticleRequest(
            "Example title",
            "Example description",
            "Example content");

        Response(
            StatusCodes.Status201Created,
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

        // TODO: validaiton also return 400

        Response(
            StatusCodes.Status500InternalServerError,
            "Internal server error response - unknown error occured.",
            example: TypedResults.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error")
                .ProblemDetails);
    }
}
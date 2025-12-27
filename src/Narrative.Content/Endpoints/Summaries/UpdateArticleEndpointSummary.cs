using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Narrative.Content.Endpoints.Summaries;

internal sealed class UpdateArticleEndpointSummary : Summary<UpdateArticleEndpoint>
{
    public UpdateArticleEndpointSummary()
    {
        Summary = "Endpoint for updating an article.";

        ExampleRequest = new UpdateArticleRequest(
            Guid.NewGuid(),
            "Example title",
            "Example description",
            "Example content");

        Response(
            StatusCodes.Status204NoContent,
            "No content response.");

        // TODO: validaiton also return 400

        Response(
            StatusCodes.Status404NotFound,
            "Not found - article not found.",
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
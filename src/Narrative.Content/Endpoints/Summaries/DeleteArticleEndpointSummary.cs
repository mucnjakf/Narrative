using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Narrative.Content.Endpoints.Summaries;

internal sealed class DeleteArticleEndpointSummary : Summary<DeleteArticleEndpoint>
{
    public DeleteArticleEndpointSummary()
    {
        Summary = "Endpoint for deleting article.";

        ExampleRequest = new DeleteArticleRequest(Guid.NewGuid());

        Response(
            StatusCodes.Status204NoContent,
            "No content response.");

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
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Narrative.Content.Enums;

namespace Narrative.Content.Endpoints.Summaries;

internal sealed class ReviewArticleEndpointSummary : Summary<ReviewArticleEndpoint>
{
    public ReviewArticleEndpointSummary()
    {
        Summary = "Endpoint for reviewing article.";

        ExampleRequest = new ReviewArticleRequest(Guid.NewGuid(), ArticleStatus.Approved);

        Response(
            StatusCodes.Status204NoContent,
            "No content response.");

        // TODO: validaiton also return 400
        Response(
            StatusCodes.Status400BadRequest,
            "Bad request response - decision cannot be used for review or article with status cannot be reviewed.",
            example: TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Bad Request")
                .ProblemDetails);

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
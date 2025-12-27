using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Narrative.Shared;

public static class ResultExtensions
{
    public static ProblemHttpResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot map problem details from successful result.");
        }

        return TypedResults.Problem(
            statusCode: GetStatusCode(result.Error.Type),
            title: GetTitle(result.Error.Type),
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { result.Error } }
            });

        static int GetStatusCode(ErrorType errorType)
            => errorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

        static string GetTitle(ErrorType errorType)
            => errorType switch
            {
                ErrorType.NotFound => "Not Found",
                ErrorType.Validation => "Bad Request",
                _ => "Internal Server Error"
            };
    }
}
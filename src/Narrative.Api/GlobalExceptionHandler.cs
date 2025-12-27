using FastEndpoints;
using Microsoft.AspNetCore.Diagnostics;

namespace Narrative.Api;

internal class GlobalExceptionHandler;

internal static class GlobalExceptionHandlerExtensions
{
    public static IApplicationBuilder UseDefaultExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async httpContext =>
            {
                var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature is not null)
                {
                    var logger = httpContext.Resolve<ILogger<GlobalExceptionHandler>>();

                    string exceptionType = exceptionHandlerFeature.Error.GetType().Name;
                    string reason = exceptionHandlerFeature.Error.Message;

                    logger.LogError(exceptionHandlerFeature.Error, "Unknown error occured.");

                    await httpContext.Response.WriteAsJsonAsync(
                        TypedResults.Problem(
                            statusCode: StatusCodes.Status500InternalServerError,
                            title: "Internal Server Error",
                            extensions: new Dictionary<string, object?>
                            {
                                { "errors", new[] { new { code = exceptionType, description = reason } } }
                            }).ProblemDetails);
                }
            });
        });

        return app;
    }
}
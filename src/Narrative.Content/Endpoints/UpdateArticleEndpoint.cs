using Ardalis.Result;
using FastEndpoints;
using Narrative.Content.Commands;
using Void = FastEndpoints.Void;

namespace Narrative.Content.Endpoints;

internal sealed record UpdateArticleRequest(Guid Id, string Title, string Description, string Content);

internal sealed class UpdateArticleEndpoint : Endpoint<UpdateArticleRequest>
{
    public override void Configure()
    {
        Put("articles/{id}");
        AllowAnonymous();
    }

    public override async Task<Void> HandleAsync(UpdateArticleRequest request, CancellationToken ct)
    {
        Result result = await new UpdateArticleCommand(request.Id, request.Title, request.Description, request.Content)
            .ExecuteAsync(ct);

        return result.Status switch
        {
            ResultStatus.NotFound => await Send.NotFoundAsync(ct), // TODO: figure out hot to return message
            _ => await Send.NoContentAsync(ct)
        };
    }
}
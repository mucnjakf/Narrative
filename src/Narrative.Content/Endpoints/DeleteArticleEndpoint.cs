using Ardalis.Result;
using FastEndpoints;
using Narrative.Content.Commands;
using Void = FastEndpoints.Void;

namespace Narrative.Content.Endpoints;

internal sealed record DeleteArticleRequest(Guid Id);

internal sealed class DeleteArticleEndpoint : Endpoint<DeleteArticleRequest>
{
    public override void Configure()
    {
        Delete("articles/{id}");
        AllowAnonymous();
    }

    public override async Task<Void> HandleAsync(DeleteArticleRequest request, CancellationToken ct)
    {
        Result result = await new DeleteArticleCommand(request.Id).ExecuteAsync(ct);

        return result.Status switch
        {
            ResultStatus.NotFound => await Send.NotFoundAsync(ct), // TODO: figure out hot to return message
            _ => await Send.NoContentAsync(ct)
        };
    }
}
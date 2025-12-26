using Ardalis.Result;
using FastEndpoints;
using Narrative.Content.Commands;
using Narrative.Content.Dtos;
using Void = FastEndpoints.Void;

namespace Narrative.Content.Endpoints;

internal sealed record CreateArticleRequest(string Title, string Description, string Content);

internal sealed record CreateArticleResponse(ArticleDto Article);

internal sealed class CreateArticleEndpoint : Endpoint<CreateArticleRequest, CreateArticleResponse>
{
    public override void Configure()
    {
        Post("articles");
        AllowAnonymous();
    }

    public override async Task<Task<Void>> HandleAsync(CreateArticleRequest request, CancellationToken ct)
    {
        Result<ArticleDto> result = await new CreateArticleCommand(request.Title, request.Description, request.Content)
            .ExecuteAsync(ct);

        return Send.CreatedAtAsync<GetArticleEndpoint>(
            new { result.Value.Id },
            new CreateArticleResponse(result.Value),
            cancellation: ct);
    }
}
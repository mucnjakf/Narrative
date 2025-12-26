using Ardalis.Result;
using FastEndpoints;
using Narrative.Content.Commands;
using Narrative.Content.Dtos;
using Void = FastEndpoints.Void;

namespace Narrative.Content.Endpoints;

internal sealed record GetAllArticlesResponse(List<ArticleDto> Articles);

internal sealed class GetAllArticlesEndpoint : EndpointWithoutRequest<GetAllArticlesResponse>
{
    public override void Configure()
    {
        Get("articles");
        AllowAnonymous();
    }

    public override async Task<Task<Void>> HandleAsync(CancellationToken ct)
    {
        Result<List<ArticleDto>> result = await new GetAllArticlesCommand().ExecuteAsync(ct);

        return Send.OkAsync(new GetAllArticlesResponse(result.Value), ct);
    }
}
using Ardalis.Result;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Narrative.Content.Commands;
using Narrative.Content.Dtos;
using Void = FastEndpoints.Void;

namespace Narrative.Content.Endpoints;

internal sealed record GetArticleRequest(Guid Id);

internal sealed record GetArticleResponse(ArticleDto Article);

internal sealed class GetArticleEndpoint : Endpoint<GetArticleRequest, GetArticleResponse>
{
    public override void Configure()
    {
        Get("articles/{id}");
        AllowAnonymous();
        Description(builder => builder.WithName("GetArticle"));
    }

    public override async Task<Void> HandleAsync(GetArticleRequest request, CancellationToken ct)
    {
        Result<ArticleDto> result = await new GetArticleCommand(request.Id).ExecuteAsync(ct);

        return result.Status switch
        {
            ResultStatus.NotFound => await Send.NotFoundAsync(ct), // TODO: figure out how to return message
            _ => await Send.OkAsync(new GetArticleResponse(result.Value), ct)
        };
    }
}
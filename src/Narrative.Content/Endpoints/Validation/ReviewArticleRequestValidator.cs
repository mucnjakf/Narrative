using FastEndpoints;
using FluentValidation;

namespace Narrative.Content.Endpoints.Validation;

internal sealed class ReviewArticleRequestValidator : Validator<ReviewArticleRequest>
{
    public ReviewArticleRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Article ID is required.");

        RuleFor(request => request.Decision)
            .IsInEnum()
            .WithMessage("Article decision is required.");
    }
}
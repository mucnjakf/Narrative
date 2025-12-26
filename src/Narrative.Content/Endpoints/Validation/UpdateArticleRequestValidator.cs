using FastEndpoints;
using FluentValidation;

namespace Narrative.Content.Endpoints.Validation;

internal sealed class UpdateArticleRequestValidator : Validator<UpdateArticleRequest>
{
    public UpdateArticleRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Article ID is required.");

        RuleFor(request => request.Title)
            .NotEmpty()
            .WithMessage("Article title is required.");

        RuleFor(request => request.Description)
            .NotEmpty()
            .WithMessage("Article description is required.");

        RuleFor(request => request.Content)
            .NotEmpty()
            .WithMessage("Article content is required.");
    }
}
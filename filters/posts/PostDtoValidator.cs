using System.Linq.Expressions;
using FluentValidation;
using reichan_api.src.DTOs.Posts;

namespace reichan_api.Filters {
    public class PostDtoValidator : AbstractValidator<PostDto>
    {
        public PostDtoValidator()
        {
            ApplyStringValidationRules(x => x.Title);
            ApplyStringValidationRules(x => x.Content);
            ApplyStringValidationRules(x => x.Author!);
            
            ApplyPublicKeyAndSignatureRules(x => x.AuthorPubKey!);
            ApplyPublicKeyAndSignatureRules(x => x.Signature!);

            RuleFor(x => x.Media)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^[a-zA-Z0-9.-]+$").WithMessage("{PropertyName} contains invalid characters.")
                .When(x => !string.IsNullOrEmpty(x.Media));
        }

        private void ApplyStringValidationRules(Expression<Func<PostDto, string>> property)
        {
            RuleFor(property)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^[a-zA-Z0-9\s.,!?'-]+$").WithMessage("{PropertyName} contains invalid characters.")
                .When(property => !string.IsNullOrEmpty(property.Author));
        }
        private void ApplyPublicKeyAndSignatureRules(Expression<Func<PostDto, string>> property)
        {
            RuleFor(property)
                .NotEmpty().WithMessage("{PropertyName} should be null of filled.")
                .Matches(@"^.+$").WithMessage("{PropertyName} is invalid.")
                .When(property => !string.IsNullOrEmpty(property.AuthorPubKey) || !string.IsNullOrEmpty(property.Signature));
        }
    }


}
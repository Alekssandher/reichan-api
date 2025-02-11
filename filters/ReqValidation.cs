namespace reichan_api.Filters {
    using System.Linq.Expressions;
    using FluentValidation;
    using reichan_api.src.DTOs.Posts;

    public class PostDtoValidator : AbstractValidator<PostDto>
    {
        public PostDtoValidator()
        {
            ApplyStringValidationRules(x => x.Title);
            ApplyStringValidationRules(x => x.Content);
            ApplyStringValidationRules(x => x.Author!);
            ApplyStringValidationRules(x => x.Media);

            ApplyPublicKeyAndSignatureRules(x => x.AuthorPubKey!);
            ApplyPublicKeyAndSignatureRules(x => x.Signature!);
        }

        private void ApplyStringValidationRules(Expression<Func<PostDto, string>> property)
        {
            RuleFor(property)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^[a-zA-Z0-9\s.,!?-]+$").WithMessage("{PropertyName} contains invalid characters.")
                .When(property => !string.IsNullOrEmpty(property.Author));
        }
        private void ApplyPublicKeyAndSignatureRules(Expression<Func<PostDto, string>> property)
        {
            RuleFor(property)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^.+$").WithMessage("{PropertyName} is invalid.")
                .When(property => !string.IsNullOrEmpty(property.AuthorPubKey) || !string.IsNullOrEmpty(property.Signature));
        }
    }


}
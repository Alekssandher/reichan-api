using System.Linq.Expressions;
using FluentValidation;
using reichan_api.src.DTOs.Threads;

namespace reichan_api.Filters.threads {
    public class ThreadDtoValidator : AbstractValidator<ThreadDto>
    {
        public ThreadDtoValidator()
        {
            ApplyStringValidationRules(x => x.Title);
            ApplyStringValidationRules(x => x.Content);
            ApplyStringValidationRules(x => x.Author!);
            

            RuleFor(x => x.Media)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^[a-z0-9/]+$").WithMessage("{PropertyName} contains invalid characters.")
                .When(x => !string.IsNullOrEmpty(x.Media));
            
        }

        private void ApplyStringValidationRules(Expression<Func<ThreadDto, string>> property)
        {
            RuleFor(property)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^[a-zA-Z0-9\s.,!?'-]+$").WithMessage("{PropertyName} contains invalid characters.")
                .When(property => !string.IsNullOrEmpty(property.Author));
        }
    }


}
using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string?> MustNotContainHtmlTags<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^[^<>]*$")
                .WithMessage("{PropertyName} contains invalid characters.");
        }
    }
}

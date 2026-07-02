using FluentValidation;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.Application.Extensions;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch
{
    public class CreateUserMatchCommandValidator : AbstractValidator<CreateUserMatchCommand>
    {
        public CreateUserMatchCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.FamilyName).NotEmpty().MustNotContainHtmlTags(nameof(CreateUserMatchCommand.FamilyName));
            RuleFor(x => x.DateOfBirth).NotEmpty();

            When(x => x.IsMatched, () =>
            {
                RuleFor(x => x.Uln).NotEmpty().WithMessage("ULN is required when matched");
                RuleFor(x => x.CertificateType).NotNull().NotEqual(CertificateType.Unknown);
                RuleFor(x => x.CourseCode).NotEmpty().MustNotContainHtmlTags(nameof(CreateUserMatchCommand.CourseCode));
                RuleFor(x => x.CourseName).NotEmpty().MustNotContainHtmlTags(nameof(CreateUserMatchCommand.CourseName));
                RuleFor(x => x.ProviderName).NotEmpty().MustNotContainHtmlTags(nameof(CreateUserMatchCommand.ProviderName));

                RuleFor(x => x.Ukprn)
                    .NotNull()
                    .WithMessage("UKPRN is required when certificate type is standard")
                    .When(x => x.CertificateType == CertificateType.Standard);

                RuleFor(x => x.Ukprn)
                    .GreaterThan(0)
                    .When(x => x.Ukprn.HasValue)
                    .WithMessage("UKPRN must be greater than 0");
            });

            RuleFor(x => x).Must(cmd => !(cmd.IsMatched && cmd.IsFailed))
                .WithMessage("A user match cannot be both matched and failed");
        }
    }
}

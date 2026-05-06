using FluentValidation;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch
{
    public class CreateUserMatchCommandValidator : AbstractValidator<CreateUserMatchCommand>
    {
        public CreateUserMatchCommandValidator()
        {
            RuleFor(x => x.FamilyName).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty();

            When(x => x.IsMatched, () =>
            {
                RuleFor(x => x.Uln).NotEmpty().WithMessage("ULN is required when matched");
                RuleFor(x => x.CertificateType).NotNull().NotEqual(CertificateType.Unknown);
                RuleFor(x => x.CourseCode).NotEmpty();
                RuleFor(x => x.CourseName).NotEmpty();
                RuleFor(x => x.ProviderName).NotEmpty();
                RuleFor(x => x.Ukprn).NotNull().GreaterThan(0);
            });
                RuleFor(x => x).Must(cmd => !(cmd.IsMatched && cmd.IsFailed))
                    .WithMessage("A user match cannot be both matched and failed");
        }
    }
}

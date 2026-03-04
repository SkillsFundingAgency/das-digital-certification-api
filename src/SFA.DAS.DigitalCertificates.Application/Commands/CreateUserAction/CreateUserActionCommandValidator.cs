using FluentValidation;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionCommandValidator : AbstractValidator<CreateUserActionCommand>
    {
        public CreateUserActionCommandValidator()
        {
            RuleFor(x => x.ActionType).IsInEnum();
            RuleFor(x => x.FamilyName).NotEmpty();
            RuleFor(x => x.GivenNames).NotEmpty();

            When(x => x.ActionType == ActionType.Reprint || x.ActionType == ActionType.Help, () =>
            {
                RuleFor(x => x.CertificateId).NotNull();
                RuleFor(x => x.CertificateType).NotNull();
                RuleFor(x => x.CertificateType)
                    .Must(type => type == CertificateType.Standard || type == CertificateType.Framework)
                    .WithMessage("CertificateType must be either Standard or Framework")
                    .When(x => x.CertificateType.HasValue);
                RuleFor(x => x.CourseName).NotEmpty();
            });
        }
    }
}

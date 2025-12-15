using FluentValidation;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing
{
    public class CreateSharingCommandValidator : AbstractValidator<CreateSharingCommand>
    {
        public CreateSharingCommandValidator()
        {
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId must not be empty");

            RuleFor(x => x.CertificateId)
            .NotEmpty().WithMessage("CertificateId must not be empty");

            RuleFor(x => x.CertificateType)
            .Must(type => type == CertificateType.Standard || type == CertificateType.Framework)
            .WithMessage("CertificateType must be either Standard or Framework");

            RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("CourseName must not be empty");
        }
    }
}

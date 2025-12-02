using FluentValidation;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing
{
    public class CreateCertificateSharingCommandValidator : AbstractValidator<CreateCertificateSharingCommand>
    {
        public CreateCertificateSharingCommandValidator()
        {
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId must not be empty");

            RuleFor(x => x.CertificateId)
            .NotEmpty().WithMessage("CertificateId must not be empty");

            RuleFor(x => x.CertificateType)
            .NotEmpty().WithMessage("CertificateType must not be empty")
            .Must(type => Enum.TryParse<Enums.CertificateType>(type, true, out _))
            .WithMessage($"CertificateType must be one of: {string.Join(", ", Enum.GetNames(typeof(Enums.CertificateType)))}");

            RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("CourseName must not be empty");
        }
    }
}

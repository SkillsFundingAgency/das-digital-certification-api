using FluentValidation;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateSharingDetails
{
    public class GetCertificateSharingDetailsQueryValidator : AbstractValidator<GetCertificateSharingDetailsQuery>
    {
        public GetCertificateSharingDetailsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID must be provided");

            RuleFor(x => x.CertificateId)
                .NotEmpty()
                .WithMessage("Certificate ID must be provided");

            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .When(x => x.Limit.HasValue)
                .WithMessage("Limit must be greater than 0");
        }
    }
}
using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharings
{
    public class GetSharingsQueryValidator : AbstractValidator<GetSharingsQuery>
    {
        public GetSharingsQueryValidator()
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
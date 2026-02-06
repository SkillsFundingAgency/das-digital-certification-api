using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode
{
    public class GetSharingByEmailLinkCodeQueryValidator : AbstractValidator<GetSharingByEmailLinkCodeQuery>
    {
        public GetSharingByEmailLinkCodeQueryValidator()
        {
            RuleFor(x => x.EmailLinkCode)
            .NotEmpty()
            .WithMessage("Email link code must be provided.");
        }
    }
}

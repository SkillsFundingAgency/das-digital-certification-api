using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode
{
    public class GetSharingByLinkCodeQueryValidator : AbstractValidator<GetSharingByLinkCodeQuery>
    {
        public GetSharingByLinkCodeQueryValidator()
        {
            RuleFor(x => x.LinkCode)
                .NotEmpty()
                .WithMessage("LinkCode cannot be empty");
        }
    }
}

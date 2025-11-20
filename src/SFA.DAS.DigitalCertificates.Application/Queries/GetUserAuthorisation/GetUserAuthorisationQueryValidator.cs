using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation
{
    public class GetUserAuthorisationQueryValidator : AbstractValidator<GetUserAuthorisationQuery>
    {
        public GetUserAuthorisationQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("UserId must be provided.");
        }
    }
}

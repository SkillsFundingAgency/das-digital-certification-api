using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity
{
    public class GetUserIdentityQueryValidator : AbstractValidator<GetUserIdentityQuery>
    {
        public GetUserIdentityQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("UserId must be provided.");
        }
    }
}

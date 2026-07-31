using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserMatches
{
    public class GetUserMatchesQueryValidator : AbstractValidator<GetUserMatchesQuery>
    {
        public GetUserMatchesQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("UserId must be provided.");
        }
    }
}

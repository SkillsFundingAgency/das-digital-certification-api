using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserById
{
    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("UserId must be provided.");
        }
    }
}

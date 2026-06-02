using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation
{
    public class CreateUserAuthorisationCommandValidator : AbstractValidator<CreateUserAuthorisationCommand>
    {
        public CreateUserAuthorisationCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Uln).GreaterThan(0);
        }
    }
}

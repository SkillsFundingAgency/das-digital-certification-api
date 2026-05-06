using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation
{
    public class CreateUserAuthorisationCommandValidator : AbstractValidator<CreateUserAuthorisationCommand>
    {
        public CreateUserAuthorisationCommandValidator()
        {
            RuleFor(x => x.Uln).GreaterThan(0);
        }
    }
}

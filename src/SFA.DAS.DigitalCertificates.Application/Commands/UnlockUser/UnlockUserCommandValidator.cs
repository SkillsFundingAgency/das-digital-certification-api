using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser
{
    public class UnlockUserCommandValidator : AbstractValidator<UnlockUserCommand>
    {
        public UnlockUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId must be supplied.");
        }
    }
}

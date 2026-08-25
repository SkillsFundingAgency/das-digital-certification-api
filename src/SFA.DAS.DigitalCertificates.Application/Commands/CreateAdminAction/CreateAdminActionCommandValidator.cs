using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction
{
    public class CreateAdminActionCommandValidator : AbstractValidator<CreateAdminActionCommand>
    {
        public CreateAdminActionCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(255)
                .Matches(@"^[\p{L}\p{N}\s\p{P}&]+$")
                .WithMessage("User name contains invalid characters.");

            RuleFor(x => x.UserActionId).GreaterThan(0);
            RuleFor(x => x.Action).IsInEnum();
        }
    }
}

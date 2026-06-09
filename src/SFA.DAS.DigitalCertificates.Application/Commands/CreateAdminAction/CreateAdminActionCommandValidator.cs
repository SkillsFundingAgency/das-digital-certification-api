using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction
{
    public class CreateAdminActionCommandValidator : AbstractValidator<CreateAdminActionCommand>
    {
        public CreateAdminActionCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.UserActionId).GreaterThan(0);
            RuleFor(x => x.Action).IsInEnum();
        }
    }
}

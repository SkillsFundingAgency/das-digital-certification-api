using FluentValidation;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandValidator : AbstractValidator<CreateOrUpdateUserCommand>
    {
        public CreateOrUpdateUserCommandValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.GovUkIdentifier)
                .NotEmpty()
                .WithMessage("GovUkIdentifier must not be empty");

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("EmailAddress must not be empty");
        }
    }
}

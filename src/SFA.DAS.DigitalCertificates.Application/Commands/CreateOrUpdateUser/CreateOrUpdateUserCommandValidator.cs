using FluentValidation;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandValidator : AbstractValidator<CreateOrUpdateUserCommand>
    {
        public CreateOrUpdateUserCommandValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.GovUkIdentifier)
                .NotEmpty().WithMessage("GovUkIdentifier must not be empty");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress must not be empty");

            RuleFor(x => x.Names)
                .NotEmpty().WithMessage("Names must have at least one entry");

            RuleFor(x => x.DateOfBirth)
                .LessThan(dateTimeProvider.Now).WithMessage("DateOfBirth cannot be in the future");
        }
    }
}

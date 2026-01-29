using FluentValidation;
using SFA.DAS.DigitalCertificates.Application.Extensions;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandValidator : AbstractValidator<CreateOrUpdateUserCommand>
    {
        public CreateOrUpdateUserCommandValidator(IDateTimeHelper dateTimeHelper)
        {
            RuleFor(x => x.GovUkIdentifier)
                .NotEmpty()
                .WithMessage("GovUkIdentifier must not be empty");

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("EmailAddress must not be empty");

            RuleFor(x => x.Names)
                .Must(names => names == null || names.Count > 0)
                .WithMessage("Names must have at least one entry if provided");

            RuleFor(x => x.DateOfBirth)
                .Must(dob => dob == null || dob < dateTimeHelper.Now)
                .WithMessage("DateOfBirth cannot be in the future");
        }
    }
}

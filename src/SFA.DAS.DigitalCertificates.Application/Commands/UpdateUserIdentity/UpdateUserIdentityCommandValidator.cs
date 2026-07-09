using FluentValidation;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity
{
    public class UpdateUserIdentityCommandValidator : AbstractValidator<UpdateUserIdentityCommand>
    {
        public UpdateUserIdentityCommandValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.Names)
                .NotEmpty()
                .WithMessage("Names must have at least one entry");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("DateOfBirth is required")
                .LessThan(dateTimeProvider.Now)
                .WithMessage("DateOfBirth cannot be in the future");
        }
    }
}
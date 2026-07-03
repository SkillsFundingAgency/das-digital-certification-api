using FluentValidation;
using SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity
{
    public class UpdateUserIdentityCommandValidator : AbstractValidator<UpdateUserIdentityCommand>
    {
        public UpdateUserIdentityCommandValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.Names)
                .Must(names => names == null || names.Count > 0)
                .WithMessage("Names must have at least one entry if provided");

            RuleFor(x => x.DateOfBirth)
                .Must(dob => dob == null || dob < dateTimeProvider.Now)
                .WithMessage("DateOfBirth cannot be in the future");
        }
    }
}

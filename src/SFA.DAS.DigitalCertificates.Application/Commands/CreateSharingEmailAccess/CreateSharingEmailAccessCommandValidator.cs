using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess
{
    public class CreateSharingEmailAccessCommandValidator : AbstractValidator<CreateSharingEmailAccessCommand>
    {
        public CreateSharingEmailAccessCommandValidator()
        {
            RuleFor(r => r.SharingEmailId)
                .NotEmpty()
                .WithMessage("SharingEmailId cannot be empty");
        }
    }
}

using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess
{
    public class CreateSharingAccessCommandValidator : AbstractValidator<CreateSharingAccessCommand>
    {
        public CreateSharingAccessCommandValidator()
        {
            RuleFor(r => r.SharingId)
                .NotEmpty()
                .WithMessage("SharingId cannot be empty");
        }
    }
}

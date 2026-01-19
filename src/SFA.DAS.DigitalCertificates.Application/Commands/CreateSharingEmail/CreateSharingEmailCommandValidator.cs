using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommandValidator : AbstractValidator<CreateSharingEmailCommand>
    {
        public CreateSharingEmailCommandValidator()
        {
            RuleFor(r => r.SharingId).NotEmpty();
            RuleFor(r => r.EmailAddress).NotEmpty().EmailAddress();
        }
    }
}

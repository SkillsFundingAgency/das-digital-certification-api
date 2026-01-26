using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommandValidator : AbstractValidator<CreateSharingEmailCommand>
    {
    public const string EmailRegex = @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z0-9_](-?[a-zA-Z0-9_])*(\.[a-zA-Z0-9](-?[a-zA-Z0-9])*)+$";

        public CreateSharingEmailCommandValidator()
        {
            RuleFor(r => r.SharingId)
                .NotEmpty();

            RuleFor(r => r.EmailAddress)
                .NotEmpty()
                .Matches(EmailRegex)
                .EmailAddress()
                .WithMessage("Invalid email address");
        }
    }
}

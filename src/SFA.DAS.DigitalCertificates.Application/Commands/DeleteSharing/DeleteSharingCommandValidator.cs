using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing
{
    public class DeleteSharingCommandValidator : AbstractValidator<DeleteSharingCommand>
    {
        public DeleteSharingCommandValidator()
        {
            RuleFor(x => x.SharingId).NotEmpty().WithMessage("SharingId must be supplied.");
        }
    }
}

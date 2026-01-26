using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommandHandler : IRequestHandler<CreateSharingEmailCommand, CreateSharingEmailCommandResponse?>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly ISharingEmailEntityContext _sharingEmailContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateSharingEmailCommandHandler(
            ISharingEntityContext sharingContext,
            ISharingEmailEntityContext sharingEmailContext,
            IDateTimeProvider dateTimeProvider)
        {
            _sharingContext = sharingContext;
            _sharingEmailContext = sharingEmailContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CreateSharingEmailCommandResponse?> Handle(CreateSharingEmailCommand request, CancellationToken cancellationToken)
        {
            var sharing = await _sharingContext.GetSharingById(request.SharingId);
            if (sharing == null)
            {
                return null;
            }

            var now = _dateTimeProvider.Now;

            var sharingEmail = new SharingEmail
            {
                SharingId = request.SharingId,
                EmailAddress = request.EmailAddress,
                EmailLinkCode = Guid.NewGuid(),
                SentTime = now
            };

            _sharingEmailContext.Add(sharingEmail);
            await _sharingEmailContext.SaveChangesAsync(cancellationToken);

            return new CreateSharingEmailCommandResponse
            {
                Id = sharingEmail.Id,
                EmailLinkCode = sharingEmail.EmailLinkCode
            };
        }
    }
}

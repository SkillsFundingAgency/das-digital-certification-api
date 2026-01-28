using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Application.Extensions;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommandHandler : IRequestHandler<CreateSharingEmailCommand, CreateSharingEmailCommandResponse?>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly ISharingEmailEntityContext _sharingEmailContext;
        private readonly IDateTimeHelper _dateTimeHelper;

        public CreateSharingEmailCommandHandler(
            ISharingEntityContext sharingContext,
            ISharingEmailEntityContext sharingEmailContext,
            IDateTimeHelper dateTimeHelper)
        {
            _sharingContext = sharingContext;
            _sharingEmailContext = sharingEmailContext;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<CreateSharingEmailCommandResponse?> Handle(CreateSharingEmailCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeHelper.Now;

            var sharing = await _sharingContext.GetSharingById(request.SharingId, now);
            if (sharing == null)
            {
                return null;
            }

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

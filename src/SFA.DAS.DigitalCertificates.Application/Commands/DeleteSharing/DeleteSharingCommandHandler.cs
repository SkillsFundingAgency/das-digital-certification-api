using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing
{
    public class DeleteSharingCommandHandler : IRequestHandler<DeleteSharingCommand, DeleteSharingCommandResponse?>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeleteSharingCommandHandler(ISharingEntityContext sharingContext, IDateTimeProvider dateTimeProvider)
        {
            _sharingContext = sharingContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<DeleteSharingCommandResponse?> Handle(DeleteSharingCommand request, CancellationToken cancellationToken)
        {
            var sharing = await _sharingContext.GetSharingByIdTracked(request.SharingId);

            if (sharing == null)
            {
                return null;
            }

            var now = _dateTimeProvider.Now;

            if (sharing.Status == SharingStatus.Deleted || sharing.ExpiryTime <= now)
            {
                return new DeleteSharingCommandResponse
                {
                    SharingId = sharing.Id
                };
            }

            sharing.Status = SharingStatus.Deleted;
            await _sharingContext.SaveChangesAsync(cancellationToken);

            return new DeleteSharingCommandResponse
            {
                SharingId = sharing.Id
            };
        }
    }
}

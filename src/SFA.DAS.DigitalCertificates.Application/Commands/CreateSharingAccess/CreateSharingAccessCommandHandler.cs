using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess
{
    public class CreateSharingAccessCommandHandler : IRequestHandler<CreateSharingAccessCommand, Guid?>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateSharingAccessCommandHandler(
            ISharingEntityContext sharingContext,
            IDateTimeProvider dateTimeProvider)
        {
            _sharingContext = sharingContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Guid?> Handle(CreateSharingAccessCommand request, CancellationToken cancellationToken)
        {
            var sharing = await _sharingContext.GetSharingByIdTracked(request.SharingId);
            if (sharing == null)
            {
                return null;
            }

            var access = new SharingAccess
            {
                SharingId = request.SharingId,
                AccessedAt = _dateTimeProvider.Now
            };

            sharing.SharingAccesses.Add(access);

            await _sharingContext.SaveChangesAsync(cancellationToken);

            return access.Id;
        }
    }
}

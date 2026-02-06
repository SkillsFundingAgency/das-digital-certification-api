using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess
{
    public class CreateSharingEmailAccessCommandHandler : IRequestHandler<CreateSharingEmailAccessCommand, System.Guid?>
    {
        private readonly ISharingEmailEntityContext _sharingEmailContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateSharingEmailAccessCommandHandler(
            ISharingEmailEntityContext sharingEmailContext,
            IDateTimeProvider dateTimeProvider)
        {
            _sharingEmailContext = sharingEmailContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Guid?> Handle(CreateSharingEmailAccessCommand request, CancellationToken cancellationToken)
        {
            var sharingEmail = await _sharingEmailContext.GetSharingEmailByIdTracked(request.SharingEmailId);
            if (sharingEmail == null)
            {
                return null;
            }

            var access = new SharingEmailAccess
            {
                SharingEmailId = request.SharingEmailId,
                AccessedAt = _dateTimeProvider.Now
            };

            if (sharingEmail.SharingEmailAccesses == null)
                sharingEmail.SharingEmailAccesses = new List<SharingEmailAccess>();

            sharingEmail.SharingEmailAccesses.Add(access);

            await _sharingEmailContext.SaveChangesAsync(cancellationToken);

            return access.Id;
        }
    }
}

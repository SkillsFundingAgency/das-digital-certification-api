using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode
{
    public class GetSharingByEmailLinkCodeQueryHandler : IRequestHandler<GetSharingByEmailLinkCodeQuery, GetSharingByEmailLinkCodeQueryResult>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetSharingByEmailLinkCodeQueryHandler(ISharingEntityContext sharingContext, IDateTimeProvider dateTimeProvider)
        {
            _sharingContext = sharingContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<GetSharingByEmailLinkCodeQueryResult> Handle(GetSharingByEmailLinkCodeQuery request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.Now;

            var sharing = await _sharingContext.GetSharingByEmailLinkCode(request.EmailLinkCode, now);

            if (sharing == null)
            {
                return new GetSharingByEmailLinkCodeQueryResult();
            }

            var sharingEmail = sharing.SharingEmails.First(se => se.EmailLinkCode == request.EmailLinkCode);

            var result = new GetSharingByEmailLinkCodeQueryResult
            {
                SharingEmail = new CertificateSharingEmailLinkSummary
                {
                    SharingEmailId = sharingEmail.Id,
                    CertificateId = sharing.CertificateId,
                    CertificateType = sharing.CertificateType,
                    ExpiryTime = sharing.ExpiryTime
                }
            };

            return result;
        }
    }
}

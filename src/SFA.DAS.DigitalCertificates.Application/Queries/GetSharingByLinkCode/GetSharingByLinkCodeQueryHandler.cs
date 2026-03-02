using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode
{
    public class GetSharingByLinkCodeQueryHandler : IRequestHandler<GetSharingByLinkCodeQuery, GetSharingByLinkCodeQueryResult>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetSharingByLinkCodeQueryHandler(ISharingEntityContext sharingContext, IDateTimeProvider dateTimeProvider)
        {
            _sharingContext = sharingContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<GetSharingByLinkCodeQueryResult> Handle(GetSharingByLinkCodeQuery request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.Now;

            var sharing = await _sharingContext.GetSharingByLinkCode(request.LinkCode, now);

            if (sharing == null)
            {
                return new GetSharingByLinkCodeQueryResult();
            }

            var result = new GetSharingByLinkCodeQueryResult
            {
                Sharing = new CertificateSharingLinkSummary
                {
                    SharingId = sharing.Id,
                    CertificateId = sharing.CertificateId,
                    CertificateType = sharing.CertificateType,
                    ExpiryTime = sharing.ExpiryTime
                }
            };

            return result;
        }
    }
}

using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateSharingDetails
{
    public class GetCertificateSharingDetailsQueryHandler : IRequestHandler<GetCertificateSharingDetailsQuery, GetCertificateSharingDetailsQueryResult>
    {
        private readonly ISharingEntityContext _sharingContext;

        public GetCertificateSharingDetailsQueryHandler(ISharingEntityContext sharingContext)
        {
            _sharingContext = sharingContext;
        }

        public async Task<GetCertificateSharingDetailsQueryResult> Handle(GetCertificateSharingDetailsQuery request, CancellationToken cancellationToken)
        {
            var allSharings = await _sharingContext.GetAllSharings(request.UserId, request.CertificateId);

            var sharingNumberMap = allSharings
                .Select((sharing, index) => new { sharing.Id, Number = index + 1 })
                .ToDictionary(x => x.Id, x => x.Number);

            var liveSharings = allSharings
                .Where(s => s.Status == SharingStatus.Live.ToString())
                .OrderByDescending(s => s.CreatedAt)
                .Take(request.Limit ?? int.MaxValue)
                .ToList();

            var certificateType = liveSharings.FirstOrDefault()?.CertificateType ?? string.Empty;
            var courseName = liveSharings.FirstOrDefault()?.CourseName ?? string.Empty;

            var sharingDetails = liveSharings.Select(s => new SharingDetail
            {
                SharingId = s.Id,
                SharingNumber = sharingNumberMap.TryGetValue(s.Id, out var number) ? number : 0,
                CreatedAt = s.CreatedAt,
                LinkCode = s.LinkCode,
                ExpiryTime = s.ExpiryTime,
                SharingAccess = s.SharingAccesses?.Select(sa => sa.AccessedAt).OrderBy(d => d).ToList(),
                SharingEmails = s.SharingEmails?.Select(se => new SharingEmailDetail
                {
                    SharingEmailId = se.Id,
                    EmailAddress = se.EmailAddress,
                    EmailLinkCode = se.EmailLinkCode,
                    SentTime = se.SentTime,
                    SharingEmailAccess = se.SharingEmailAccesses?.Select(sea => sea.AccessedAt).OrderBy(d => d).ToList()
                }).ToList()
            }).ToList();

            return new GetCertificateSharingDetailsQueryResult
            {
                SharingDetails = new CertificateSharingDetails
                {
                    UserId = request.UserId,
                    CertificateId = request.CertificateId,
                    CourseName = courseName,
                    CertificateType = certificateType,
                    Sharings = sharingDetails
                }
            };
        }
    }
}

using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById
{
    public class GetSharingByIdQueryHandler : IRequestHandler<GetSharingByIdQuery, GetSharingByIdQueryResult>
    {
        private readonly ISharingEntityContext _sharingContext;

        public GetSharingByIdQueryHandler(ISharingEntityContext sharingContext)
        {
            _sharingContext = sharingContext;
        }

        public async Task<GetSharingByIdQueryResult> Handle(GetSharingByIdQuery request, CancellationToken cancellationToken)
        {
            var sharing = await _sharingContext.GetSharingById(request.SharingId);

            if (sharing == null)
            {
                return new GetSharingByIdQueryResult
                {
                    Sharing = null
                };
            }

            var allSharings = await _sharingContext.GetAllSharingsBasic(sharing.UserId, sharing.CertificateId);

            var sharingNumberMap = allSharings
                .OrderBy(s => s.CreatedAt)
                .Select((s, index) => new { s.Id, Number = index + 1 })
                .ToDictionary(x => x.Id, x => x.Number);

            var sharingNumber = sharingNumberMap.TryGetValue(sharing.Id, out var number) ? number : 0;

            var sharingAccess = sharing.SharingAccesses?
                .OrderByDescending(sa => sa.AccessedAt)
                .Take(request.Limit ?? int.MaxValue)
                .Select(sa => sa.AccessedAt)
                .ToList();

            var sharingEmails = sharing.SharingEmails?
                .OrderByDescending(se => se.SentTime)
                .Take(request.Limit ?? int.MaxValue)
                .Select(se => new SharingEmailDetail
                {
                    SharingEmailId = se.Id,
                    EmailAddress = se.EmailAddress,
                    EmailLinkCode = se.EmailLinkCode,
                    SentTime = se.SentTime,
                    SharingEmailAccess = se.SharingEmailAccesses?
                        .OrderByDescending(sea => sea.AccessedAt)
                        .Take(request.Limit ?? int.MaxValue)
                        .Select(sea => sea.AccessedAt)
                        .ToList()
                })
                .ToList();

            var certificateSharing = new CertificateSharing
            {
                UserId = sharing.UserId,
                CertificateId = sharing.CertificateId,
                CertificateType = sharing.CertificateType,
                CourseName = sharing.CourseName,
                SharingId = sharing.Id,
                SharingNumber = sharingNumber,
                CreatedAt = sharing.CreatedAt,
                LinkCode = sharing.LinkCode,
                ExpiryTime = sharing.ExpiryTime,
                SharingAccess = sharingAccess,
                SharingEmails = sharingEmails
            };

            return new GetSharingByIdQueryResult
            {
                Sharing = certificateSharing
            };
        }
    }
}
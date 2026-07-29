using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetSharingByLinkCodeResponse
    {
        public Guid SharingId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public DateTime ExpiryTime { get; set; }

        public static implicit operator GetSharingByLinkCodeResponse(Domain.Models.CertificateSharingLinkSummary source)
        {
            if (source == null) return null!;

            return new GetSharingByLinkCodeResponse
            {
                SharingId = source.SharingId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                ExpiryTime = source.ExpiryTime
            };
        }
    }
}

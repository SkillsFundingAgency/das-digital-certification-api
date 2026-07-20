using System;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetSharingByEmailLinkCodeResponse
    {
        public Guid SharingEmailId { get; set; }
        public Guid CertificateId { get; set; }
        public Enums.CertificateType CertificateType { get; set; }
        public DateTime ExpiryTime { get; set; }

        public static implicit operator GetSharingByEmailLinkCodeResponse(Domain.Models.CertificateSharingEmailLinkSummary source)
        {
            if (source == null) return null!;

            return new GetSharingByEmailLinkCodeResponse
            {
                SharingEmailId = source.SharingEmailId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                ExpiryTime = source.ExpiryTime
            };
        }
    }
}

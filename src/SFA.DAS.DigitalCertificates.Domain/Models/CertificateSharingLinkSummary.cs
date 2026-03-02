using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.Models
{
    public class CertificateSharingLinkSummary
    {
        public Guid SharingId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}

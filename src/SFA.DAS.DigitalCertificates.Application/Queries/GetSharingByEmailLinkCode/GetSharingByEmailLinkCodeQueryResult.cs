using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode
{
    public class GetSharingByEmailLinkCodeQueryResult
    {
        public CertificateSharingEmailLinkSummary? SharingEmail { get; set; }
    }
}

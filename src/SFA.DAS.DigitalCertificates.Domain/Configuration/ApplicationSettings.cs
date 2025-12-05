using SFA.DAS.Api.Common.Configuration;

namespace SFA.DAS.DigitalCertificates.Domain.Configuration
{
    public class ApplicationSettings
    {
        public AzureActiveDirectoryConfiguration? AzureAd { get; set; }

        public string? DbConnectionString { get; set; }

        public int CertificateSharingExpiryDays { get; set; } = 28;
    }
}

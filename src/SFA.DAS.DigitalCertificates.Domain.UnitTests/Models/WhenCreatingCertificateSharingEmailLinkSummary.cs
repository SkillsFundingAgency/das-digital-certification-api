using FluentAssertions;
using NUnit.Framework;
using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Models
{
    public class WhenCreatingCertificateSharingEmailLinkSummary
    {
        [Test]
        public void Then_PropertiesAreSetCorrectly()
        {
            var sharingEmailId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var certificateType = CertificateType.Standard;
            var expiryTime = DateTime.UtcNow.AddDays(5);

            var model = new SFA.DAS.DigitalCertificates.Domain.Models.CertificateSharingEmailLinkSummary
            {
                SharingEmailId = sharingEmailId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                ExpiryTime = expiryTime
            };

            model.SharingEmailId.Should().Be(sharingEmailId);
            model.CertificateId.Should().Be(certificateId);
            model.CertificateType.Should().Be(certificateType);
            model.ExpiryTime.Should().Be(expiryTime);
        }
    }
}

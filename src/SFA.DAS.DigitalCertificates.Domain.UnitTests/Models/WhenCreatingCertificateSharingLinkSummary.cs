using FluentAssertions;
using NUnit.Framework;
using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Models
{
    public class WhenCreatingCertificateSharingLinkSummary
    {
        [Test]
        public void Then_PropertiesAreSetCorrectly()
        {
            var sharingId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var certificateType = CertificateType.Framework;
            var expiryTime = DateTime.UtcNow.AddDays(3);

            var model = new SFA.DAS.DigitalCertificates.Domain.Models.CertificateSharingLinkSummary
            {
                SharingId = sharingId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                ExpiryTime = expiryTime
            };

            model.SharingId.Should().Be(sharingId);
            model.CertificateId.Should().Be(certificateId);
            model.CertificateType.Should().Be(certificateType);
            model.ExpiryTime.Should().Be(expiryTime);
        }
    }
}

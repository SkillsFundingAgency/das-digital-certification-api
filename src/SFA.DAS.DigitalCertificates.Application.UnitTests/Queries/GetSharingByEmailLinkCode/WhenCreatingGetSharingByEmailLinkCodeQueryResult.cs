using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingByEmailLinkCode
{
    [TestFixture]
    public class WhenCreatingGetSharingByEmailLinkCodeQueryResult
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            var sharingEmail = new CertificateSharingEmailLinkSummary
            {
                SharingEmailId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = Enums.CertificateType.Standard,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            var result = new GetSharingByEmailLinkCodeQueryResult
            {
                SharingEmail = sharingEmail
            };

            result.SharingEmail.Should().Be(sharingEmail);
        }
    }
}

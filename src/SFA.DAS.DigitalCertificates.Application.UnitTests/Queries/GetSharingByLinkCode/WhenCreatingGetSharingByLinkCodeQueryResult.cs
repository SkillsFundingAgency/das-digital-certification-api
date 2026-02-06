using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode;
using SFA.DAS.DigitalCertificates.Domain.Models;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;
using System;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingByLinkCode
{
    [TestFixture]
    public class WhenCreatingGetSharingByLinkCodeQueryResult
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            var sharing = new CertificateSharingLinkSummary
            {
                SharingId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            var result = new GetSharingByLinkCodeQueryResult
            {
                Sharing = sharing
            };

            result.Sharing.Should().Be(sharing);
        }
    }
}

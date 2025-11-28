using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateSharingDetails;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetCertificateSharingDetails
{
    [TestFixture]
    public class WhenCreatingGetCertificateSharingDetailsQueryResult
    {
        [Test]
        public void And_DefaultConstructor_Then_SharingDetailsIsNull()
        {
            var result = new GetCertificateSharingDetailsQueryResult();
            result.SharingDetails.Should().BeNull();
        }

        [Test]
        public void And_SharingDetailsIsSet_Then_PropertiesAreMapped()
        {
            var details = new CertificateSharingDetails
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "TypeA",
                Sharings = new List<SharingDetail>()
            };
            var result = new GetCertificateSharingDetailsQueryResult { SharingDetails = details };
            result.SharingDetails.Should().Be(details);
        }
    }
}

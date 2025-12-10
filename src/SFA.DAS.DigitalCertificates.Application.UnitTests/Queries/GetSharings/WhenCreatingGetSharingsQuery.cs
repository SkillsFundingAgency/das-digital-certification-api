using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharings
{
    [TestFixture]
    public class WhenCreatingGetSharingsQuery
    {
        [Test]
        public void And_DefaultValues_Then_PropertiesAreNullOrDefault()
        {
            var query = new GetSharingsQuery();
            query.UserId.Should().Be(Guid.Empty);
            query.CertificateId.Should().Be(Guid.Empty);
            query.Limit.Should().BeNull();
        }

        [Test]
        public void And_PropertiesSet_Then_ValuesAreMapped()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var query = new GetSharingsQuery { UserId = userId, CertificateId = certId, Limit = 5 };
            query.UserId.Should().Be(userId);
            query.CertificateId.Should().Be(certId);
            query.Limit.Should().Be(5);
        }
    }
}

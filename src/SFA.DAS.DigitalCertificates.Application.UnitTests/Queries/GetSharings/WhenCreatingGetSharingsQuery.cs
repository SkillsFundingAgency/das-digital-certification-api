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
            // Arrange

            // Act
            var query = new GetSharingsQuery();

            // Assert
            query.UserId.Should().Be(Guid.Empty);
            query.CertificateId.Should().Be(Guid.Empty);
            query.Limit.Should().BeNull();
        }

        [Test]
        public void And_PropertiesSet_Then_ValuesAreMapped()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();

            // Act
            var query = new GetSharingsQuery { UserId = userId, CertificateId = certId, Limit = 5 };

            // Assert
            query.UserId.Should().Be(userId);
            query.CertificateId.Should().Be(certId);
            query.Limit.Should().Be(5);
        }
    }
}

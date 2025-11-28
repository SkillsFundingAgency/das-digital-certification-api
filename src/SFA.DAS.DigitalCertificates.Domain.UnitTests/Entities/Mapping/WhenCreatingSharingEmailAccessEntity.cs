using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingSharingEmailAccessEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var sharingEmailId = Guid.NewGuid();
            var accessedAt = DateTime.UtcNow;

            var access = new SharingEmailAccess
            {
                Id = id,
                SharingEmailId = sharingEmailId,
                AccessedAt = accessedAt,
                SharingEmail = new SharingEmail { Id = sharingEmailId, EmailAddress = "test@example.com", EmailLinkCode = Guid.NewGuid(), SentTime = DateTime.UtcNow, Sharing = new Sharing { Id = Guid.NewGuid(), CertificateType = "TestType", Status = "Active" } }
            };

            access.Id.Should().Be(id);
            access.SharingEmailId.Should().Be(sharingEmailId);
            access.AccessedAt.Should().Be(accessedAt);
            access.SharingEmail.Should().NotBeNull();
            access.SharingEmail.Id.Should().Be(sharingEmailId);
        }
    }
}

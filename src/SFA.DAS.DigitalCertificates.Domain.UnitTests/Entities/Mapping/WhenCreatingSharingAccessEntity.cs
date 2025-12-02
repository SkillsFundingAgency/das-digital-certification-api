using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingSharingAccessEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var sharingId = Guid.NewGuid();
            var accessedAt = DateTime.UtcNow;

            var access = new SharingAccess
            {
                Id = id,
                SharingId = sharingId,
                AccessedAt = accessedAt,
                Sharing = new Sharing { Id = sharingId, CertificateType = "TestType", CourseName = "CourseName", Status = "Active" }
            };

            access.Id.Should().Be(id);
            access.SharingId.Should().Be(sharingId);
            access.AccessedAt.Should().Be(accessedAt);
            access.Sharing.Should().NotBeNull();
            access.Sharing.Id.Should().Be(sharingId);
        }
    }
}

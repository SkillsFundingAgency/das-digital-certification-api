using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingSharingEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var certificateType = "TestType";
            var courseName = "CourseName";
            var linkCode = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;
            var expiry = createdAt.AddDays(1);
            var status = "Active";

            var sharing = new Sharing
            {
                Id = id,
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName,
                LinkCode = linkCode,
                CreatedAt = createdAt,
                ExpiryTime = expiry,
                Status = status,
                SharingAccesses = new List<SharingAccess>(),
                SharingEmails = new List<SharingEmail>()
            };

            sharing.Id.Should().Be(id);
            sharing.UserId.Should().Be(userId);
            sharing.CertificateId.Should().Be(certificateId);
            sharing.CertificateType.Should().Be(certificateType);
            sharing.LinkCode.Should().Be(linkCode);
            sharing.CreatedAt.Should().Be(createdAt);
            sharing.ExpiryTime.Should().Be(expiry);
            sharing.Status.Should().Be(status);
            sharing.SharingAccesses.Should().NotBeNull();
            sharing.SharingEmails.Should().NotBeNull();
        }
    }
}

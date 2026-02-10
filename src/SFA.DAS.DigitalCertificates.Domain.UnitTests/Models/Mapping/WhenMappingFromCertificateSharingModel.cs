using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Models
{
    public class WhenMappingFromCertificateSharingModel
    {
        [Test]
        public void ThenTheFieldsAreCorrectlyMapped()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var certificateType = CertificateType.Standard;
            var courseName = "TestCourse";
            var sharingId = Guid.NewGuid();
            var sharingNumber = 1;
            var createdAt = DateTime.UtcNow;
            var linkCode = Guid.NewGuid();
            var expiryTime = DateTime.UtcNow.AddDays(7);
            var sharingAccess = new List<DateTime> { DateTime.UtcNow };
            var sharingEmails = new List<Domain.Models.SharingEmailDetail>
            {
                new Domain.Models.SharingEmailDetail
                {
                    SharingEmailId = Guid.NewGuid(),
                    EmailAddress = "test@example.com",
                    EmailLinkCode = Guid.NewGuid(),
                    SentTime = DateTime.UtcNow,
                    SharingEmailAccess = new List<DateTime> { DateTime.UtcNow }
                }
            };

            // Act
            var model = new Domain.Models.CertificateSharing
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName,
                SharingId = sharingId,
                SharingNumber = sharingNumber,
                CreatedAt = createdAt,
                LinkCode = linkCode,
                ExpiryTime = expiryTime,
                SharingAccess = sharingAccess,
                SharingEmails = sharingEmails
            };

            // Assert
            model.UserId.Should().Be(userId);
            model.CertificateId.Should().Be(certificateId);
            model.CertificateType.Should().Be(certificateType);
            model.CourseName.Should().Be(courseName);
            model.SharingId.Should().Be(sharingId);
            model.SharingNumber.Should().Be(sharingNumber);
            model.CreatedAt.Should().Be(createdAt);
            model.LinkCode.Should().Be(linkCode);
            model.ExpiryTime.Should().Be(expiryTime);
            model.SharingAccess.Should().BeEquivalentTo(sharingAccess);
            model.SharingEmails.Should().BeEquivalentTo(sharingEmails);
        }
    }
}

using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateCertificateSharing
{
    public class WhenCreatingCertificateSharingCommandResponse
    {
        [Test]
        public void And_PropertiesAreSet_Then_ResponseIsCorrect()
        {
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var sharingId = Guid.NewGuid();
            var linkCode = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var expiry = now.AddDays(28);
            var response = new CreateCertificateSharingCommandResponse
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = "Standard",
                CourseName = "Test Course",
                SharingId = sharingId,
                SharingNumber = 1,
                CreatedAt = now,
                LinkCode = linkCode,
                ExpiryTime = expiry,
                SharingAccess = new List<object>(),
                SharingEmails = new List<object>()
            };
            response.UserId.Should().Be(userId);
            response.CertificateId.Should().Be(certificateId);
            response.CertificateType.Should().Be("Standard");
            response.CourseName.Should().Be("Test Course");
            response.SharingId.Should().Be(sharingId);
            response.SharingNumber.Should().Be(1);
            response.CreatedAt.Should().Be(now);
            response.LinkCode.Should().Be(linkCode);
            response.ExpiryTime.Should().Be(expiry);
            response.SharingAccess.Should().BeEmpty();
            response.SharingEmails.Should().BeEmpty();
        }
    }
}

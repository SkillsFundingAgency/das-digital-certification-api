using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharing
{
    public class WhenCreatingSharingCommandResponse
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
            var response = new CreateSharingCommandResponse
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                SharingId = sharingId,
                SharingNumber = 1,
                CreatedAt = now,
                LinkCode = linkCode,
                ExpiryTime = expiry
            };
            response.UserId.Should().Be(userId);
            response.CertificateId.Should().Be(certificateId);
            response.CertificateType.Should().Be(CertificateType.Standard);
            response.CourseName.Should().Be("Test Course");
            response.SharingId.Should().Be(sharingId);
            response.SharingNumber.Should().Be(1);
            response.CreatedAt.Should().Be(now);
            response.LinkCode.Should().Be(linkCode);
            response.ExpiryTime.Should().Be(expiry);
        }
    }
}

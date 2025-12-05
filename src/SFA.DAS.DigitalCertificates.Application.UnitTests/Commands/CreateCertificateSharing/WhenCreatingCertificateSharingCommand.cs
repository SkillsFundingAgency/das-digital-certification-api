using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateCertificateSharing
{
    public class WhenCreatingCertificateSharingCommand
    {
        [Test]
        public void And_PropertiesAreSet_Then_CommandIsCorrect()
        {
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var command = new CreateCertificateSharingCommand
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = "Standard",
                CourseName = "Test Course"
            };
            command.UserId.Should().Be(userId);
            command.CertificateId.Should().Be(certificateId);
            command.CertificateType.Should().Be("Standard");
            command.CourseName.Should().Be("Test Course");
        }
    }
}

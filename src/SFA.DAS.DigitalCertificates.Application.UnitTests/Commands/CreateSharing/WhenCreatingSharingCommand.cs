using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharing
{
    public class WhenCreatingSharingCommand
    {
        [Test]
        public void And_PropertiesAreSet_Then_CommandIsCorrect()
        {
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var command = new CreateSharingCommand
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };
            command.UserId.Should().Be(userId);
            command.CertificateId.Should().Be(certificateId);
            command.CertificateType.Should().Be(CertificateType.Standard);
            command.CourseName.Should().Be("Test Course");
        }
    }
}

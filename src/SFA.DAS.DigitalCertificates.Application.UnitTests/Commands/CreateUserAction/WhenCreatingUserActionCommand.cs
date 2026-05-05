using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserAction
{
    public class WhenCreatingUserActionCommand
    {
        [Test]
        public void And_PropertiesAreSet_Then_CommandIsCorrect()
        {
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();

            var command = new CreateUserActionCommand
            {
                UserId = userId,
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            command.UserId.Should().Be(userId);
            command.ActionType.Should().Be(ActionType.Reprint);
            command.FamilyName.Should().Be("Smith");
            command.GivenNames.Should().Be("John");
            command.CertificateId.Should().Be(certificateId);
            command.CertificateType.Should().Be(CertificateType.Standard);
            command.CourseName.Should().Be("Test Course");
        }

        [Test]
        public void And_NoCertificateFields_Then_CommandIsCorrect()
        {
            var userId = Guid.NewGuid();

            var command = new CreateUserActionCommand
            {
                UserId = userId,
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = "John"
            };

            command.UserId.Should().Be(userId);
            command.ActionType.Should().Be(ActionType.Contact);
            command.CertificateId.Should().BeNull();
            command.CertificateType.Should().BeNull();
            command.CourseName.Should().BeNull();
        }
    }
}

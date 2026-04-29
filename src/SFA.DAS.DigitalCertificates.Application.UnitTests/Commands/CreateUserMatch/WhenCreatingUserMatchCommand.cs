using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserMatch
{
    public class WhenCreatingUserMatchCommand
    {
        [Test]
        public void And_AllPropertiesAreSet_Then_CommandIsCorrect()
        {
            var userId = Guid.NewGuid();
            var dateOfBirth = new DateTime(1990, 1, 1);
            var dateAwarded = 2022;

            var command = new CreateUserMatchCommand
            {
                UserId = userId,
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = dateOfBirth,
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                CourseLevel = 2,
                DateAwarded = dateAwarded,
                ProviderName = "Test Provider",
                Ukprn = 123456,
                IsMatched = true,
                IsFailed = false
            };

            command.UserId.Should().Be(userId);
            command.Uln.Should().Be(1234567890);
            command.FamilyName.Should().Be("Smith");
            command.DateOfBirth.Should().Be(dateOfBirth);
            command.CertificateType.Should().Be(CertificateType.Standard);
            command.CourseCode.Should().Be("C123");
            command.CourseName.Should().Be("Test Course");
            command.CourseLevel.Should().Be(2);
            command.DateAwarded.Should().Be(dateAwarded);
            command.ProviderName.Should().Be("Test Provider");
            command.Ukprn.Should().Be(123456);
            command.IsMatched.Should().BeTrue();
            command.IsFailed.Should().BeFalse();
        }
    }
}

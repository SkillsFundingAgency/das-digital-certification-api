using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingUserMatchEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var uln = 9999999999L;
            var familyName = "Smith";
            var dob = new DateTime(1985, 5, 5);
            var certificateType = CertificateType.Standard;
            var courseCode = "C123";
            var courseName = "Course Name";
            var courseLevel = 3;
            var dateAwarded = DateTime.UtcNow.AddDays(-365).Year;
            var providerName = "Provider Ltd";
            var ukprn = 123456;

            var match = new UserMatch
            {
                Id = id,
                UserId = userId,
                Uln = uln,
                FamilyName = familyName,
                DateOfBirth = dob,
                CertificateType = certificateType,
                CourseCode = courseCode,
                CourseName = courseName,
                CourseLevel = courseLevel,
                DateAwarded = dateAwarded,
                ProviderName = providerName,
                Ukprn = ukprn,
                IsMatched = true,
                IsFailed = false
            };

            match.Id.Should().Be(id);
            match.UserId.Should().Be(userId);
            match.Uln.Should().Be(uln);
            match.FamilyName.Should().Be(familyName);
            match.DateOfBirth.Should().Be(dob);
            match.CertificateType.Should().Be(certificateType);
            match.CourseCode.Should().Be(courseCode);
            match.CourseName.Should().Be(courseName);
            match.CourseLevel.Should().Be(courseLevel);
            match.DateAwarded.Should().Be(dateAwarded);
            match.ProviderName.Should().Be(providerName);
            match.Ukprn.Should().Be(ukprn);
            match.IsMatched.Should().BeTrue();
            match.IsFailed.Should().BeFalse();
        }
    }
}

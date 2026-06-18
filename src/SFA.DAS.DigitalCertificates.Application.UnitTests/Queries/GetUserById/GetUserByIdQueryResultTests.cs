using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserById;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserById
{
    public class GetUserByIdQueryResultTests
    {
        [Test]
        public void Properties_Are_Mapped_Correctly()
        {
            var matches = new[] {
                new UserMatchDetail { Id = Guid.NewGuid(), Uln = 111111, FamilyName = "Smith", DateOfBirth = new DateTime(1990,1,1), CertificateType = "Standard", CourseCode = "C1", CourseName = "Course", CourseLevel = "3", DateAwarded = 2020, ProviderName = "P", Ukprn = 123, IsMatched = true, IsFailed = false }
            };

            var r = new GetUserByIdQueryResult
            {
                UserId = Guid.NewGuid(),
                GovUkIdentifier = "G1",
                EmailAddress = "a@b.com",
                PhoneNumber = "01234",
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                IsLocked = false,
                UserMatches = matches
            };

            r.UserMatches.Should().BeEquivalentTo(matches);
            r.GovUkIdentifier.Should().Be("G1");
            r.EmailAddress.Should().Be("a@b.com");
            r.PhoneNumber.Should().Be("01234");
            r.IsLocked.Should().BeFalse();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserMatches;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserMatches
{
    public class GetUserMatchesQueryHandlerTests
    {
        [Test]
        public async Task When_User_Exists_With_Matches_Returns_User_And_Matches()
        {
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                GovUkIdentifier = "G1",
                EmailAddress = "a@b.com",
                PhoneNumber = "012345",
                LastLoginAt = DateTime.UtcNow,
                IsLocked = false,
                UserIdentities = Array.Empty<UserIdentity>(),
                UserMatches = new List<UserMatch>
                {
                    new UserMatch
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Uln = 1234567890,
                        FamilyName = "Smith",
                        DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        CertificateType = CertificateType.Standard,
                        CourseCode = "C1",
                        CourseName = "Course",
                        CourseLevel = "3",
                        YearAwarded = 2020,
                        ProviderName = "Provider",
                        Ukprn = 12345,
                        IsMatched = true,
                        IsFailed = false
                    }
                }
            };

            var userEntityMock = new Mock<IUserEntityContext>();
            userEntityMock.Setup(x => x.GetWithMatchesByUserId(userId)).ReturnsAsync(user);

            var sut = new GetUserMatchesQueryHandler(userEntityMock.Object);

            var result = await sut.Handle(new GetUserMatchesQuery { UserId = userId }, CancellationToken.None);

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.GovUkIdentifier.Should().Be("G1");
            result.EmailAddress.Should().Be("a@b.com");
            result.UserMatches.Should().NotBeNull();
            result.UserMatches.Should().HaveCount(1);
            var um = Enumerable.First(result.UserMatches);
            um.Uln.Should().Be(1234567890);
            um.FamilyName.Should().Be("Smith");
            um.CertificateType.Should().Be(CertificateType.Standard);
        }

        [Test]
        public void When_User_Does_Not_Exist_Throws_ValidationException()
        {
            var userId = Guid.NewGuid();

            var userEntityMock = new Mock<IUserEntityContext>();
            userEntityMock.Setup(x => x.GetWithIdentitiesAndAuthorisationByUserId(userId)).ReturnsAsync((User?)null);

            var sut = new GetUserMatchesQueryHandler(userEntityMock.Object);

            Func<Task> act = async () => await sut.Handle(new GetUserMatchesQuery { UserId = userId }, CancellationToken.None);

            act.Should().ThrowAsync<ValidationException>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserById;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserById
{
    public class GetUserByIdQueryHandlerTests
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
                UserIdentities = Array.Empty<UserIdentity>()
            };

            var match = new UserMatch
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = Domain.Models.Enums.CertificateType.Standard,
                CourseCode = "C1",
                CourseName = "Course",
                CourseLevel = "3",
                YearAwarded = 2020,
                ProviderName = "Provider",
                Ukprn = 12345,
                IsMatched = true,
                IsFailed = false
            };

            var userEntityMock = new Mock<IUserEntityContext>();
            userEntityMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(userId)).ReturnsAsync(user);

            var matchMock = new Mock<IUserMatchEntityContext>();
            matchMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<UserMatch> { match });

            var sut = new GetUserByIdQueryHandler(userEntityMock.Object, matchMock.Object);

            var result = await sut.Handle(new GetUserByIdQuery { UserId = userId }, CancellationToken.None);

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.GovUkIdentifier.Should().Be("G1");
            result.EmailAddress.Should().Be("a@b.com");
            result.UserMatches.Should().NotBeNull();
            result.UserMatches.Should().HaveCount(1);
            var um = Enumerable.First(result.UserMatches);
            um.Uln.Should().Be(1234567890);
            um.FamilyName.Should().Be("Smith");
            um.CertificateType.Should().Be("Standard");
        }

        [Test]
        public void When_User_Does_Not_Exist_Throws_ValidationException()
        {
            var userId = Guid.NewGuid();

            var userEntityMock = new Mock<IUserEntityContext>();
            userEntityMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(userId)).ReturnsAsync((User?)null);

            var matchMock = new Mock<IUserMatchEntityContext>();

            var sut = new GetUserByIdQueryHandler(userEntityMock.Object, matchMock.Object);

            Func<Task> act = async () => await sut.Handle(new GetUserByIdQuery { UserId = userId }, CancellationToken.None);

            act.Should().ThrowAsync<ValidationException>();
        }
    }
}

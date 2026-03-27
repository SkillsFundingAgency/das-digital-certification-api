using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserIdentity
{
    public class GetUserIdentityQueryHandlerTests
    {
        [Test]
        public async Task When_User_Is_Authorised_Authorisation_Is_Populated_And_Excluded_Is_Empty()
        {
            var userId = Guid.NewGuid();
            var dob = new DateTime(1990,1,1);

            var identity = new UserIdentity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FamilyName = "Smith",
                GivenNames = "Joe",
                DateOfBirth = dob,
                ValidSince = DateTime.UtcNow
            };

            var user = new User
            {
                Id = userId,
                GovUkIdentifier = "G1",
                EmailAddress = "a@b.com",
                UserIdentities = new[] { identity },
                UserAuthorisation = new UserAuthorisation { Id = Guid.NewGuid(), ULN = 1234567890, AuthorisedAt = DateTime.UtcNow }
            };

            var userEntityMock = new Mock<IUserEntityContext>();
            userEntityMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(userId)).ReturnsAsync(user);

            var matchMock = new Mock<IUserMatchEntityContext>();
            var authMock = new Mock<IUserAuthorisationEntityContext>();

            var sut = new GetUserIdentityQueryHandler(userEntityMock.Object, matchMock.Object, authMock.Object);

            var result = await sut.Handle(new GetUserIdentityQuery { UserId = userId }, CancellationToken.None);

            result.Authorisation.Should().NotBeNull();
            result.Excluded.Should().BeEmpty();
        }

        [Test]
        public async Task When_User_Is_Not_Authorised_Then_Excluded_Populated_From_Matches()
        {
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            var dob = new DateTime(1990,1,1);

            var identity = new UserIdentity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FamilyName = "Smith",
                GivenNames = "Joe",
                DateOfBirth = dob,
                ValidSince = DateTime.UtcNow
            };

            var user = new User
            {
                Id = userId,
                GovUkIdentifier = "G1",
                EmailAddress = "a@b.com",
                UserIdentities = new[] { identity },
                UserAuthorisation = null
            };

            var userEntityMock = new Mock<IUserEntityContext>();
            userEntityMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(userId)).ReturnsAsync(user);

            var matchMock = new Mock<IUserMatchEntityContext>();
            matchMock.Setup(x => x.GetPreviouslyAuthorisedUlns("Smith", dob)).ReturnsAsync(new List<Guid> { userId, otherUserId });

            var authMock = new Mock<IUserAuthorisationEntityContext>();
            authMock.Setup(x => x.GetAuthorisedUlns(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<long> { 555 });

            var sut = new GetUserIdentityQueryHandler(userEntityMock.Object, matchMock.Object, authMock.Object);

            var result = await sut.Handle(new GetUserIdentityQuery { UserId = userId }, CancellationToken.None);

            result.Authorisation.Should().BeNull();
            result.Excluded.Should().ContainSingle().Which.Should().Be(555);
        }
    }
}

using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity;
using SFA.DAS.DigitalCertificates.Domain.Models;
using SFA.DAS.DigitalCertificates.Application.Models;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserIdentity
{
    public class GetUserIdentityQueryResultTests
    {
        [Test]
        public void And_IdentityIsSet_Then_PropertiesAreMapped()
        {
            // Arrange
            var names = new[] {
                new Name { FamilyName = "Smith", GivenNames = "Joe", ValidSince = new DateTime(2020,1,1) },
                new Name { FamilyName = "Jones", GivenNames = "Ann", ValidSince = new DateTime(2019,1,1) }
            };

            var dob = new DateTime(1970,1,1);

            var auth = new UserAuthorisation { AuthorisationId = Guid.NewGuid(), ULN = 123456, AuthorisedAt = DateTime.UtcNow };

            var excluded = new List<long> { 111, 222 };

            // Act
            var r = new GetUserIdentityQueryResult
            {
                Identity = names,
                DateOfBirth = dob,
                Authorisation = auth,
                Excluded = excluded
            };

            // Assert
            r.Identity.Should().BeEquivalentTo(names);
            r.DateOfBirth.Should().Be(dob);
            r.Authorisation.Should().Be(auth);
            r.Excluded.Should().BeEquivalentTo(excluded);
        }
    }
}

using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingUserIdentityEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var givenNames = "John";
            var familyName = "Doe";
            var dob = new DateTime(1990, 1, 1);
            var validSince = DateTime.UtcNow.AddDays(-10);
            var validUntil = DateTime.UtcNow.AddDays(10);

            var identity = new UserIdentity
            {
                Id = id,
                UserId = userId,
                GivenNames = givenNames,
                FamilyName = familyName,
                DateOfBirth = dob,
                ValidSince = validSince,
                ValidUntil = validUntil
            };

            identity.Id.Should().Be(id);
            identity.UserId.Should().Be(userId);
            identity.GivenNames.Should().Be(givenNames);
            identity.FamilyName.Should().Be(familyName);
            identity.DateOfBirth.Should().Be(dob);
            identity.ValidSince.Should().Be(validSince);
            identity.ValidUntil.Should().Be(validUntil);
        }
    }
}

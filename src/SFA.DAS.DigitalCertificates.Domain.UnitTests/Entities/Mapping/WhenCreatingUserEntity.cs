using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingUserEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var govUk = "G123";
            var email = "test@example.com";
            var phone = "0123456789";
            var lastLogin = DateTime.UtcNow;
            var isLocked = true;

            var user = new User
            {
                Id = id,
                GovUkIdentifier = govUk,
                EmailAddress = email,
                PhoneNumber = phone,
                LastLoginAt = lastLogin,
                IsLocked = isLocked
            };

            user.Id.Should().Be(id);
            user.GovUkIdentifier.Should().Be(govUk);
            user.EmailAddress.Should().Be(email);
            user.PhoneNumber.Should().Be(phone);
            user.LastLoginAt.Should().Be(lastLogin);
            user.IsLocked.Should().BeTrue();
        }
    }
}

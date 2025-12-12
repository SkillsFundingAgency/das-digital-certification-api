using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Models
{
    public class WhenMappingFromUserEntityToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(User source)
        {
            // Act
            var result = (Domain.Models.User?)source;

            // Act
            result.Should().NotBeNull();
            result!.Id.Should().Be(source.Id);
            result!.GovUkIdentifier.Should().Be(source.GovUkIdentifier);
            result!.EmailAddress.Should().Be(source.EmailAddress);
            result!.PhoneNumber.Should().Be(source.PhoneNumber);
            result!.LastLoginAt.Should().Be(source.LastLoginAt);
            result!.IsLocked.Should().Be(source.IsLocked);
        }
    }
}

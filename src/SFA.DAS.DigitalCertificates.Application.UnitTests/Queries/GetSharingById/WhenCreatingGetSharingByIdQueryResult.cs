using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.Domain.Models;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingById
{
    [TestFixture]
    public class WhenCreatingGetSharingByIdQueryResult
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            // Arrange
            var sharing = new CertificateSharing
            {
                UserId = System.Guid.NewGuid(),
                CertificateId = System.Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                SharingId = System.Guid.NewGuid(),
                SharingNumber = 1,
                CreatedAt = System.DateTime.UtcNow,
                LinkCode = System.Guid.NewGuid(),
                ExpiryTime = System.DateTime.UtcNow.AddDays(30)
            };

            // Act
            var result = new GetSharingByIdQueryResult
            {
                Sharing = sharing
            };

            // Assert
            result.Sharing.Should().Be(sharing);
        }
    }
}
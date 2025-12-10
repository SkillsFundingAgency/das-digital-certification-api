using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingById
{
    [TestFixture]
    public class WhenCreatingGetSharingByIdQueryResult
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            var sharing = new CertificateSharing
            {
                UserId = System.Guid.NewGuid(),
                CertificateId = System.Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course",
                SharingId = System.Guid.NewGuid(),
                SharingNumber = 1,
                CreatedAt = System.DateTime.UtcNow,
                LinkCode = System.Guid.NewGuid(),
                ExpiryTime = System.DateTime.UtcNow.AddDays(30)
            };

            var result = new GetSharingByIdQueryResult
            {
                Sharing = sharing
            };

            result.Sharing.Should().Be(sharing);
        }
    }
}
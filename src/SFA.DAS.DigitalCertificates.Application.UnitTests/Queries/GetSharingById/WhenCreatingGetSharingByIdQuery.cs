using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingById
{
    [TestFixture]
    public class WhenCreatingGetSharingByIdQuery
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var limit = 10;

            // Act
            var query = new GetSharingByIdQuery
            {
                SharingId = sharingId,
                Limit = limit
            };

            // Assert
            query.SharingId.Should().Be(sharingId);
            query.Limit.Should().Be(limit);
        }

        [Test]
        public void Then_CanCreateWithoutLimit()
        {
            // Arrange
            var sharingId = Guid.NewGuid();

            // Act
            var query = new GetSharingByIdQuery
            {
                SharingId = sharingId
            };

            // Assert
            query.SharingId.Should().Be(sharingId);
            query.Limit.Should().BeNull();
        }
    }
}
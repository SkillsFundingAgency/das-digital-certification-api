using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserActions
{
    [TestFixture]
    public class WhenCreatingGetUserActionsQuery
    {
        [Test]
        public void And_DefaultValues_Then_PropertiesAreNullOrDefault()
        {
            // Act
            var query = new GetUserActionsQuery();

            // Assert
            query.UserId.Should().Be(Guid.Empty);
        }

        [Test]
        public void And_PropertiesSet_Then_ValuesAreMapped()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var query = new GetUserActionsQuery { UserId = userId };

            // Assert
            query.UserId.Should().Be(userId);
        }
    }
}

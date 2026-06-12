using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAction;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserAction
{
    [TestFixture]
    public class WhenCreatingGetUserActionByCodeQueryResult
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var actionTime = DateTime.UtcNow;

            // Act
            var result = new GetUserActionByCodeQueryResult
            {
                Id = 1,
                UserId = userId,
                ActionType = ActionType.Reprint,
                FamilyName = "A",
                GivenNames = "B",
                ActionTime = actionTime,
                Uln = 12345678
            };

            // Assert
            result.Id.Should().Be(1);
            result.UserId.Should().Be(userId);
            result.ActionType.Should().Be(ActionType.Reprint);
            result.FamilyName.Should().Be("A");
            result.GivenNames.Should().Be("B");
            result.ActionTime.Should().Be(actionTime);
            result.Uln.Should().Be(12345678);
        }
    }
}

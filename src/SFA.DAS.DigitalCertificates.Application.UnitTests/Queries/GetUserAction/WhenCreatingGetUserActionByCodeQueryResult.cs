using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAction;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
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
            var action = new UserActionDetail { Id = 1, UserId = Guid.NewGuid(), ActionType = ActionType.Reprint, FamilyName = "A", GivenNames = "B", ActionTime = DateTime.UtcNow, Uln = 12345678 };

            // Act
            var result = new GetUserActionByCodeQueryResult
            {
                UserAction = action
            };

            // Assert
            result.UserAction.Should().BeEquivalentTo(action);
        }
    }
}

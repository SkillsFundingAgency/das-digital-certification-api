using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserActions
{
    [TestFixture]
    public class WhenCreatingGetUserActionsQueryResult
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            // Arrange
            var actions = new List<UserActionDetail>
            {
                new UserActionDetail { Id = 1, UserId = Guid.NewGuid(), ActionType = ActionType.Reprint, FamilyName = "A", GivenNames = "B", ActionTime = DateTime.UtcNow, Uln = 12345678 }
            };

            // Act
            var result = new GetUserActionsQueryResult
            {
                UserActions = actions
            };

            // Assert
            result.UserActions.Should().BeEquivalentTo(actions);
        }
    }
}

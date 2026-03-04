using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingAdminActionsEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var username = "testuser";
            var actionTime = DateTime.UtcNow;
            var action = AdminActionType.Viewed;
            var userActionId = 123L;
            var userAction = new UserActions
            {
                Id = userActionId,
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John"
            };

            var adminActions = new AdminActions
            {
                Id = id,
                Username = username,
                ActionTime = actionTime,
                Action = action,
                UserActionId = userActionId,
                UserAction = userAction
            };

            adminActions.Id.Should().Be(id);
            adminActions.Username.Should().Be(username);
            adminActions.ActionTime.Should().Be(actionTime);
            adminActions.Action.Should().Be(action);
            adminActions.UserActionId.Should().Be(userActionId);
            adminActions.UserAction.Should().Be(userAction);
        }

        [Test]
        public void And_NoUserAction_Then_UserActionIsNull()
        {
            var id = Guid.NewGuid();

            var adminActions = new AdminActions
            {
                Id = id,
                Username = "testuser",
                ActionTime = DateTime.UtcNow,
                Action = AdminActionType.Unlocked,
                UserActionId = 456L
            };

            adminActions.UserAction.Should().BeNull();
        }
    }
}

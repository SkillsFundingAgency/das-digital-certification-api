using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingUserActionsEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = 1L;
            var userId = Guid.NewGuid();
            var actionType = ActionType.Reprint;
            var familyName = "Smith";
            var givenNames = "John";
            var certificateId = Guid.NewGuid();
            var certificateType = CertificateType.Standard;
            var courseName = "Test Course";
            var actionCode = "ABC123";
            var actionTime = DateTime.UtcNow;
            var user = new User { Id = userId, GovUkIdentifier = "gov-uk-id", EmailAddress = "test@test.com" };
            var adminActions = new List<AdminActions>
            {
                new AdminActions { Id = Guid.NewGuid(), Username = "admin", ActionTime = actionTime, Action = AdminActionType.Viewed, UserActionId = id }
            };

            var userActions = new UserActions
            {
                Id = id,
                UserId = userId,
                User = user,
                ActionType = actionType,
                FamilyName = familyName,
                GivenNames = givenNames,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName,
                ActionCode = actionCode,
                ActionTime = actionTime,
                AdminActions = adminActions
            };

            userActions.Id.Should().Be(id);
            userActions.UserId.Should().Be(userId);
            userActions.User.Should().Be(user);
            userActions.ActionType.Should().Be(actionType);
            userActions.FamilyName.Should().Be(familyName);
            userActions.GivenNames.Should().Be(givenNames);
            userActions.CertificateId.Should().Be(certificateId);
            userActions.CertificateType.Should().Be(certificateType);
            userActions.CourseName.Should().Be(courseName);
            userActions.ActionCode.Should().Be(actionCode);
            userActions.ActionTime.Should().Be(actionTime);
            userActions.AdminActions.Should().BeEquivalentTo(adminActions);
        }

        [Test]
        public void And_OptionalFieldsNotSet_Then_TheyAreNull()
        {
            var userActions = new UserActions
            {
                ActionType = ActionType.Help,
                FamilyName = "Jones",
                GivenNames = "Jane"
            };

            userActions.User.Should().BeNull();
            userActions.CertificateId.Should().BeNull();
            userActions.CertificateType.Should().BeNull();
            userActions.CourseName.Should().BeNull();
            userActions.ActionCode.Should().BeNull();
        }

        [Test]
        public void And_NoAdminActions_Then_AdminActionsIsEmpty()
        {
            var userActions = new UserActions
            {
                ActionType = ActionType.Contact,
                FamilyName = "Brown",
                GivenNames = "Alice"
            };

            userActions.AdminActions.Should().NotBeNull();
            userActions.AdminActions.Should().BeEmpty();
        }
    }
}

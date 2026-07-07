using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAction;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserAction
{
    [TestFixture]
    public class WhenHandlingGetUserActionByCodeQueryHandler
    {
        private Mock<IUserActionsEntityContext> _userActionsContextMock = null!;
        private GetUserActionByCodeQueryHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _userActionsContextMock = new Mock<IUserActionsEntityContext>();

            _sut = new GetUserActionByCodeQueryHandler(_userActionsContextMock.Object);
        }

        [Test]
        public async Task And_NoAction_Then_ReturnsNull()
        {
            // Arrange
            var actionCode = "NOEXIST";
            _userActionsContextMock.Setup(x => x.GetByActionCodeAsync(actionCode, It.IsAny<CancellationToken>())).ReturnsAsync((UserActions?)null);

            // Act
            var query = new GetUserActionByCodeQuery { ActionCode = actionCode };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.UserAction.Should().BeNull();
            _userActionsContextMock.Verify(x => x.GetByActionCodeAsync(actionCode, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ActionWithoutAdminActions_Then_ActionStatusIsNew()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var actionCode = "AC1";

            var ua = new UserActions
            {
                Id = 1,
                UserId = userId,
                ActionType = ActionType.Reprint,
                ActionTime = DateTime.UtcNow.AddMinutes(-10),
                FamilyName = "Smith",
                GivenNames = "John",
                ActionCode = actionCode,
                User = new User
                {
                    Id = userId,
                    GovUkIdentifier = "GOV123",
                    EmailAddress = "test@example.com",
                    UserAuthorisation = new UserAuthorisation { Id = Guid.NewGuid(), UserId = userId, ULN = 12345678, AuthorisedAt = DateTime.UtcNow }
                }
            };

            _userActionsContextMock.Setup(x => x.GetByActionCodeAsync(actionCode, It.IsAny<CancellationToken>())).ReturnsAsync(ua);

            // Act
            var query = new GetUserActionByCodeQuery { ActionCode = actionCode };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.UserAction.Should().NotBeNull();
            var detail = result.UserAction!;
            detail.Id.Should().Be(1);
            detail.FamilyName.Should().Be("Smith");
            detail.GivenNames.Should().Be("John");
            detail.ActionStatus.Should().Be(UserActionStatus.New);
            detail.AdminActions.Should().BeEmpty();
            detail.Uln.Should().Be(12345678);
            _userActionsContextMock.Verify(x => x.GetByActionCodeAsync(actionCode, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ActionWithAdminActions_Then_ActionStatusViewedAndAdminActionsOrdered()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var actionCode = "AC2";

            var admin1 = new AdminActions { Id = Guid.NewGuid(), Username = "admin1", Action = AdminActionType.Viewed, ActionTime = now.AddMinutes(-5), UserActionId = 1 };
            var admin2 = new AdminActions { Id = Guid.NewGuid(), Username = "admin2", Action = AdminActionType.Unlocked, ActionTime = now.AddMinutes(-1), UserActionId = 1 };

            var ua = new UserActions
            {
                Id = 1,
                UserId = userId,
                ActionType = ActionType.Reprint,
                ActionTime = now.AddMinutes(-10),
                FamilyName = "Jones",
                GivenNames = "Amy",
                ActionCode = actionCode,
                AdminActions = new List<AdminActions> { admin1, admin2 },
                User = new User
                {
                    Id = userId,
                    GovUkIdentifier = "GOV123",
                    EmailAddress = "test@example.com",
                    UserAuthorisation = new UserAuthorisation { Id = Guid.NewGuid(), UserId = userId, ULN = 12345678, AuthorisedAt = DateTime.UtcNow }
                }
            };

            _userActionsContextMock.Setup(x => x.GetByActionCodeAsync(actionCode, It.IsAny<CancellationToken>())).ReturnsAsync(ua);

            // Act
            var query = new GetUserActionByCodeQuery { ActionCode = actionCode };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.UserAction.Should().NotBeNull();
            var detail = result.UserAction!;
            detail.ActionStatus.Should().Be(UserActionStatus.Viewed);
            detail.AdminActions.Should().NotBeNull();
            detail.AdminActions!.Should().HaveCount(2);
            detail.AdminActions.First().Username.Should().Be("admin2");
            detail.AdminActions.Last().Username.Should().Be("admin1");
            detail.Uln.Should().Be(12345678);
            _userActionsContextMock.Verify(x => x.GetByActionCodeAsync(actionCode, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

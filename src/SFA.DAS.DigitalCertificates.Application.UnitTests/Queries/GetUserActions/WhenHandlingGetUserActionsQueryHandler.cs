using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserActions
{
    [TestFixture]
    public class WhenHandlingGetUserActionsQueryHandler
    {
        private Mock<IUserActionsEntityContext> _userActionsContextMock = null!;
        private GetUserActionsQueryHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _userActionsContextMock = new Mock<IUserActionsEntityContext>();

            _sut = new GetUserActionsQueryHandler(_userActionsContextMock.Object);
        }

        [Test]
        public async Task And_NoActions_Then_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userActionsContextMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<UserActions>());

            // Act
            var query = new GetUserActionsQuery { UserId = userId };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.UserActions.Should().NotBeNull();
            result.UserActions.Should().BeEmpty();
            _userActionsContextMock.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ActionsWithoutAdminActions_Then_ActionStatusIsNew()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var actions = new List<UserActions>
            {
                new UserActions
                {
                    Id = 1,
                    UserId = userId,
                    ActionType = ActionType.Reprint,
                    ActionTime = DateTime.UtcNow.AddMinutes(-10),
                    FamilyName = "Smith",
                    GivenNames = "John",
                    ActionCode = "AC1"
                }
            };

            _userActionsContextMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(actions);

            // Act
            var query = new GetUserActionsQuery { UserId = userId };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.UserActions.Should().HaveCount(1);
            var ua = result.UserActions.First();
            ua.Id.Should().Be(1);
            ua.FamilyName.Should().Be("Smith");
            ua.GivenNames.Should().Be("John");
            ua.ActionStatus.Should().Be(UserActionStatus.New);
            ua.AdminActions.Should().BeEmpty();
            _userActionsContextMock.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ActionsWithAdminActions_Then_ActionStatusViewedAndAdminActionsOrdered()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var admin1 = new AdminActions { Id = Guid.NewGuid(), Username = "admin1", Action = AdminActionType.Viewed, ActionTime = now.AddMinutes(-5), UserActionId = 1 };
            var admin2 = new AdminActions { Id = Guid.NewGuid(), Username = "admin2", Action = AdminActionType.Unlocked, ActionTime = now.AddMinutes(-1), UserActionId = 1 };

            var actions = new List<UserActions>
            {
                new UserActions
                {
                    Id = 1,
                    UserId = userId,
                    ActionType = ActionType.Reprint,
                    ActionTime = now.AddMinutes(-10),
                    FamilyName = "Jones",
                    GivenNames = "Amy",
                    ActionCode = "AC2",
                    AdminActions = new List<AdminActions> { admin1, admin2 }
                }
            };

            _userActionsContextMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(actions);

            // Act
            var query = new GetUserActionsQuery { UserId = userId };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.UserActions.Should().HaveCount(1);
            var ua = result.UserActions.First();
            ua.ActionStatus.Should().Be(UserActionStatus.Viewed);
            ua.AdminActions.Should().NotBeNull();
            ua.AdminActions!.Should().HaveCount(2);
            // Admin actions should be ordered descending by ActionTime
            ua.AdminActions.First().Username.Should().Be("admin2");
            ua.AdminActions.Last().Username.Should().Be("admin1");
            _userActionsContextMock.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.Encoding;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserAction
{
    public class WhenHandlingCreateUserActionCommandHandler
    {
        private Mock<IUserActionsEntityContext> _userActionsContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private Mock<IEncodingService> _encodingServiceMock = null!;
        private CreateUserActionCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _userActionsContextMock = new Mock<IUserActionsEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _encodingServiceMock = new Mock<IEncodingService>();
            _sut = new CreateUserActionCommandHandler(
                _userActionsContextMock.Object,
                _dateTimeProviderMock.Object,
                _encodingServiceMock.Object);
        }

        [Test]
        public async Task And_NoExistingAction_Then_CreatesNewActionAndReturnsNewStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var expectedCode = "ACTION123";

            _userActionsContextMock
                .Setup(x => x.GetMostRecentActionAsync(userId, ActionType.Contact, null))
                .ReturnsAsync((UserActions?)null);

            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _userActionsContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _encodingServiceMock.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.SupportReference)).Returns(expectedCode);

            var command = new CreateUserActionCommand
            {
                UserId = userId,
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = "John"
            };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.ActionCode.Should().Be(expectedCode);
            _userActionsContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Test]
        public async Task And_ExistingActionWithNoAdminActions_Then_ReturnsExistingCodeWithNewStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingCode = "EXISTING123";

            var existing = new UserActions
            {
                Id = 1,
                UserId = userId,
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                ActionCode = existingCode,
                ActionTime = DateTime.UtcNow,
                AdminActions = new List<AdminActions>()
            };

            _userActionsContextMock
                .Setup(x => x.GetMostRecentActionAsync(userId, ActionType.Reprint, It.IsAny<Guid?>()))
                .ReturnsAsync(existing);

            var command = new CreateUserActionCommand
            {
                UserId = userId,
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.ActionCode.Should().Be(existingCode);
            _userActionsContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_ExistingActionWithAdminActions_Then_CreatesNewActionAndReturnsNewStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var existingCode = "EXISTING123";
            var newCode = "NEW456";

            var existing = new UserActions
            {
                Id = 1,
                UserId = userId,
                ActionType = ActionType.Help,
                FamilyName = "Smith",
                GivenNames = "John",
                ActionCode = existingCode,
                ActionTime = DateTime.UtcNow,
                AdminActions = new List<AdminActions>
                {
                    new AdminActions { Id = Guid.NewGuid(), Username = "admin", Action = AdminActionType.Viewed, ActionTime = DateTime.UtcNow, UserActionId = 1 }
                }
            };

            _userActionsContextMock
                .Setup(x => x.GetMostRecentActionAsync(userId, ActionType.Help, certificateId))
                .ReturnsAsync(existing);

            _dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
            _userActionsContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _encodingServiceMock.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.SupportReference)).Returns(newCode);

            var command = new CreateUserActionCommand
            {
                UserId = userId,
                ActionType = ActionType.Help,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.ActionCode.Should().Be(newCode);
            _userActionsContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}
